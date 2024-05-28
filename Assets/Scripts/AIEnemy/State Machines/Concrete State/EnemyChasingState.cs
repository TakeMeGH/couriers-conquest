using System.Collections;
using System.Collections.Generic;
using CC.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace CC.Enemy.States
{
    public class EnemyChasingState : EnemyControllerState
    {
        float timeToSetDestination = 0;
        bool isOutOfRange = false;
        public EnemyChasingState(EnemyController enemy) : base(enemy)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation("isSlowChasing");

            isOutOfRange = false;
            CheckChase();

            _enemyController.NavMeshAgent.isStopped = false;
            _enemyController.NavMeshAgent.angularSpeed = _enemyController.EnemyPersistenceData.DefaultAnggularSpeed;
            _enemyController.NavMeshAgent.speed = _enemyController.EnemyPersistenceData.SlowChaseSpeed;
            _enemyController.NavMeshAgent.stoppingDistance = _enemyController.EnemyPersistenceData.DefaultStopingDistance;
        }


        public override void Update()
        {
            base.Update();

        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            timeToSetDestination -= Time.deltaTime;
            if (timeToSetDestination <= 0)
            {

                CheckChase();
                timeToSetDestination = _enemyController.EnemyPersistenceData.TimeToReChase;
            }

            float distance = Vector3.Distance(_enemyController.transform.position,
                _enemyController.EnemyCurrentData.PlayerTransform.transform.position);

            if (distance < _enemyController.EnemyPersistenceData.ChaseStopingDistance)
            {
                _enemyController.SwitchState(_enemyController.StepBackState);
            }
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation("isSlowChasing");

        }

        void CheckChase()
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(_enemyController.EnemyCurrentData.PlayerTransform.position, out hit, 1.0f, NavMesh.AllAreas))
            {
                _enemyController.NavMeshAgent.SetDestination(_enemyController.EnemyCurrentData.PlayerTransform.position);
                isOutOfRange = false;
            }
            else
            {
                if (!isOutOfRange && NavMesh.SamplePosition(_enemyController.EnemyCurrentData.PlayerTransform.position, out hit, 10.0f, NavMesh.AllAreas))
                {
                    _enemyController.NavMeshAgent.SetDestination(hit.position);
                    isOutOfRange = true;
                }
            }

            if (isOutOfRange && !_enemyController.NavMeshAgent.pathPending && _enemyController.NavMeshAgent.remainingDistance <= _enemyController.NavMeshAgent.stoppingDistance)
            {
                _enemyController.SwitchState(_enemyController.TauntState);
            }
        }
    }
}