using System.Collections;
using System.Collections.Generic;
using CC.Characters;
using CC.Enemy;
using CC.StateMachine;
using UnityEngine;
using UnityEngine.AI;

namespace CC.Enemy.States
{
    public class EnemyControllerState : IState
    {
        protected EnemyController _enemyController;

        public EnemyControllerState(EnemyController _enemyController)
        {
            this._enemyController = _enemyController;
        }

        public virtual void Enter()
        {
            if (_enemyController.EnemyCurrentData.PlayerTransform == null)
            {
                _enemyController.EnemyCurrentData.PlayerTransform = GameObject.FindObjectOfType<PlayerControllerStatesMachine>()?.transform;
            }
        }


        public virtual void Update()
        {
            EnviromentView();
        }

        public virtual void PhysicsUpdate()
        {
            _enemyController.Animator.SetFloat("Velocity", _enemyController.NavMeshAgent.velocity.sqrMagnitude);

        }

        public virtual void Exit()
        {

        }

        public virtual void OnAnimationEnterEvent()
        {
        }

        public virtual void OnAnimationExitEvent()
        {
        }

        public virtual void OnAnimationTransitionEvent()
        {
        }

        protected void StartAnimation(string transitionName)
        {
            _enemyController.Animator.SetBool(transitionName, true);
        }

        protected void StopAnimation(string transitionName)
        {
            _enemyController.Animator.SetBool(transitionName, false);
        }

        protected bool IsStoping()
        {
            if (!_enemyController.NavMeshAgent.pathPending)
            {
                if (_enemyController.NavMeshAgent.remainingDistance <= _enemyController.NavMeshAgent.stoppingDistance)
                {
                    if (!_enemyController.NavMeshAgent.hasPath || _enemyController.NavMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public virtual void OnAttacked()
        {
            _enemyController.EnemyCurrentData.AttackedCount++;
            if (_enemyController.EnemyCurrentData.AttackedCount == _enemyController.EnemyPersistenceData.AttackedLimitBeforeStuned)
            {
                _enemyController.SwitchState(_enemyController.StunedState);
            }
        }

        public void EnviromentView()
        {
            if (_enemyController.EnemyCurrentData.IsPlayerInRange)
            {
                return;
            }
            Vector3 offsetEnemyPosition = _enemyController.transform.position + _enemyController.EnemyPersistenceData.EnemyRayOffset;

            Collider[] playerInRange = Physics.OverlapSphere(offsetEnemyPosition, _enemyController.EnemyPersistenceData.ViewRadius, _enemyController.EnemyPersistenceData.PlayerMask);
            for (int i = 0; i < playerInRange.Length; i++)
            {
                Transform player = playerInRange[i].transform;

                Vector3 dirToPlayer = (player.position - offsetEnemyPosition).normalized;

                if (Vector3.Angle(_enemyController.transform.forward, dirToPlayer) < _enemyController.EnemyPersistenceData.ViewAngle / 2)
                {
                    float dstToPlayer = Vector3.Distance(offsetEnemyPosition, player.position);

                    if (!Physics.Raycast(offsetEnemyPosition, dirToPlayer, dstToPlayer, _enemyController.EnemyPersistenceData.ObstacleMask)
                        && IsInNavmeshSurface(player))
                    {
                        _enemyController.EnemyCurrentData.PlayerTransform = player;
                        _enemyController.EnemyCurrentData.IsPlayerInRange = true;
                    }
                }
            }
        }

        public bool IsInNavmeshSurface(Transform player)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(player.position, out hit, _enemyController.EnemyPersistenceData.NavmeshRayMaxDistance, NavMesh.AllAreas))
            {
                return true;
            }
            return false;

        }
    }
}
