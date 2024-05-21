using UnityEngine;
using System;
using UnityEngine.AI;


namespace CC.Enemy.States
{
    public class EnemyPatrolingState : EnemyControllerState
    {
        public NavMeshPath navMeshPath;

        public EnemyPatrolingState(EnemyController _enemyController) : base(_enemyController)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation("isPatroling");

            _enemyController.NavMeshAgent.isStopped = false;
            _enemyController.NavMeshAgent.speed = _enemyController.EnemyPersistenceData.PatrolSpeed;
            _enemyController.NavMeshAgent.stoppingDistance = _enemyController.EnemyPersistenceData.PatrolStopDistance;
            _enemyController.NavMeshAgent.SetDestination(GetRandomWayPointDestination());

            _enemyController.EnemyCurrentData.IsPlayerInRange = false;
        }

        public override void Update()
        {
            base.Update();

            if (_enemyController.EnemyCurrentData.IsPlayerInRange)
            {
                _enemyController.SwitchState(_enemyController.FastTauntingState);
            }

            if (IsStoping())
            {
                _enemyController.SwitchState(_enemyController.CheckingState);
            }

        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation("isPatroling");

        }

        private Vector3 GetRandomWayPointDestination()
        {
            System.Random random = new System.Random();
            int randomIndex = random.Next(_enemyController.PatrolWaypoints.Length);

            while (randomIndex == _enemyController.EnemyCurrentData.CurrentWaypointIndex)
            {
                randomIndex = random.Next(_enemyController.PatrolWaypoints.Length);
            }
            _enemyController.EnemyCurrentData.CurrentWaypointIndex = randomIndex;
            return _enemyController.PatrolWaypoints[randomIndex].position;
        }

    }
}
