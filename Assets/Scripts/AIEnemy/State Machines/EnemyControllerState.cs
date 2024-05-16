using System.Collections;
using System.Collections.Generic;
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

        }


        public virtual void Update()
        {
            EnviromentView();

            // if (enemy.Animator != null)
            // {
            //     enemy.Animator.SetFloat("Speed", enemy.EnemyCurrentData.navMeshAgent.velocity.magnitude);
            // }
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



        // public void Move(float speed)
        // {
        //     enemy.EnemyCurrentData.navMeshAgent.isStopped = false;
        //     enemy.EnemyCurrentData.navMeshAgent.speed = speed;
        // }

        // public void Stop()
        // {
        //     enemy.EnemyCurrentData.navMeshAgent.isStopped = true;
        //     enemy.EnemyCurrentData.navMeshAgent.speed = 0;
        // }
        // public void NextPoint(bool useRandom = true)
        // {
        //     if (useRandom)
        //     {
        //         enemy.EnemyCurrentData.m_CurrentWaypointIndex = Random.Range(0, enemy.waypoints.Length);
        //     }

        //     enemy.EnemyCurrentData.navMeshAgent.SetDestination(enemy.waypoints[enemy.EnemyCurrentData.m_CurrentWaypointIndex].position);

        // }

        void CaughtPlayer()
        {
            // enemy.EnemyCurrentData.m_CaughtPlayer = true;
        }

        // public void LookingPlayer(Vector3 player)
        // {
        //     enemy.EnemyCurrentData.navMeshAgent.SetDestination(player);
        //     if (Vector3.Distance(enemy.transform.position, player) <= 0.3)
        //     {
        //         if (enemy.EnemyCurrentData.m_WaitTime <= 0)
        //         {
        //             enemy.EnemyCurrentData.m_PlayerNear = false;
        //             Move(enemy.EnemyCurrentData.speedWalk);
        //             enemy.EnemyCurrentData.navMeshAgent.SetDestination(enemy.waypoints[enemy.EnemyCurrentData.m_CurrentWaypointIndex].position);
        //             enemy.EnemyCurrentData.m_WaitTime = enemy.EnemyCurrentData.startWaitTime;
        //             enemy.EnemyCurrentData.m_TimeToRotate = enemy.EnemyCurrentData.timetoRotate;
        //         }
        //         else
        //         {
        //             Stop();
        //             enemy.EnemyCurrentData.m_WaitTime -= Time.deltaTime;
        //         }
        //     }

        //     Vector3 direction = (player - enemy.transform.position).normalized;
        //     Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        //     enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, lookRotation, Time.deltaTime * enemy.EnemyCurrentData.rotationSpeed);
        // }
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
