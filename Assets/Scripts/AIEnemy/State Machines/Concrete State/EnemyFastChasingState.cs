using System.Collections;
using System.Collections.Generic;
using CC.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace CC.Enemy.States
{
    public class EnemyFastChasingState : EnemyControllerState
    {
        public EnemyFastChasingState(EnemyController enemy) : base(enemy)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation("isFastChasing");

            CheckChase();

            _enemyController.NavMeshAgent.isStopped = false;
            _enemyController.NavMeshAgent.angularSpeed = _enemyController.EnemyPersistenceData.DefaultAnggularSpeed;
            _enemyController.NavMeshAgent.speed = _enemyController.EnemyPersistenceData.FastChaseSpeed;
            _enemyController.NavMeshAgent.stoppingDistance = _enemyController.EnemyPersistenceData.AttackChaseDistancce;
        }


        public override void Update()
        {
            base.Update();

        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (IsStoping())
            {
                _enemyController.SwitchState(_enemyController.AttackState);
            }
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation("isFastChasing");

        }

        void CheckChase()
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(_enemyController.EnemyCurrentData.PlayerTransform.position, out hit, 1.0f, NavMesh.AllAreas))
            {
                _enemyController.NavMeshAgent.SetDestination(_enemyController.EnemyCurrentData.PlayerTransform.position);
            }
            else
            {
                if (NavMesh.SamplePosition(_enemyController.EnemyCurrentData.PlayerTransform.position, out hit, 10.0f, NavMesh.AllAreas))
                {
                    _enemyController.NavMeshAgent.SetDestination(hit.position);
                }
            }

        }
    }
}