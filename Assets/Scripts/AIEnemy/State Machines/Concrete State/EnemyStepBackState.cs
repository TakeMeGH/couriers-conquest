using UnityEngine;
using System;
using UnityEngine.AI;


namespace CC.Enemy.States
{
    public class EnemyStepBackState : EnemyControllerState
    {
        public NavMeshPath navMeshPath;
        float timeToSetDestination = 0;
        float stepBackTime = 0;


        public EnemyStepBackState(EnemyController _enemyController) : base(_enemyController)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation("isStepBack");

            stepBackTime = 0;
            timeToSetDestination = 0;

            _enemyController.NavMeshAgent.isStopped = false;
            _enemyController.NavMeshAgent.angularSpeed = 0;

            _enemyController.NavMeshAgent.speed = _enemyController.EnemyPersistenceData.StepBackSpeed;
            _enemyController.NavMeshAgent.stoppingDistance = _enemyController.EnemyPersistenceData.DefaultStopingDistance;
        }

        public override void Update()
        {
            base.Update();

            stepBackTime += Time.deltaTime;

            if (stepBackTime >= _enemyController.EnemyPersistenceData.StepBackMaxTime)
            {
                _enemyController.SwitchState(_enemyController.IdleAttackState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            float distance = Vector3.Distance(_enemyController.transform.position,
    _enemyController.EnemyCurrentData.PlayerTransform.transform.position);

            if (distance > _enemyController.EnemyPersistenceData.StepBackDistance)
            {
                _enemyController.SwitchState(_enemyController.IdleAttackState);
            }

            timeToSetDestination -= Time.deltaTime;
            if (timeToSetDestination <= 0)
            {

                StepBack();
                timeToSetDestination = _enemyController.EnemyPersistenceData.TimeToReStepBack;
            }


        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation("isStepBack");

        }

        private void StepBack()
        {
            LookAt(_enemyController.EnemyCurrentData.PlayerTransform);

            Vector3 directionToPlayer = _enemyController.transform.position -
                _enemyController.EnemyCurrentData.PlayerTransform.position;

            Vector3 backwardDirection = directionToPlayer.normalized;
            Vector3 backwardPosition = _enemyController.transform.position + backwardDirection;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(backwardPosition, out hit, 1.0f, NavMesh.AllAreas))
            {
                _enemyController.NavMeshAgent.SetDestination(hit.position);
            }

        }
    }
}
