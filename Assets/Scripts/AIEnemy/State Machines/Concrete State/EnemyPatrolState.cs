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
            // navMeshPath = new NavMeshPath();
            // Debug.Log(_enemyController.NavMeshAgent.pathStatus + " DEBUG");
            // Debug.Log(_enemyController.NavMeshAgent.CalculatePath(GetRandomWayPointDestination(), navMeshPath) + " " + navMeshPath.status);
        }

        public override void Update()
        {
            base.Update();

            if (_enemyController.EnemyCurrentData.IsPlayerInRange)
            {
                _enemyController.SwitchState(_enemyController.ChasingState);
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

            while (Vector3.Distance(_enemyController.PatrolWaypoints[randomIndex].position,
                _enemyController.transform.position) <= _enemyController.NavMeshAgent.stoppingDistance)
            {
                randomIndex = random.Next(_enemyController.PatrolWaypoints.Length);
            }

            return _enemyController.PatrolWaypoints[randomIndex].position;
        }


        // public override void OnAnimationEnterEvent()
        // {
        //     base.OnAnimationEnterEvent();
        // }

        // private void Patroling()
        // {
        //         if (enemy.EnemyCurrentData.navMeshAgent.remainingDistance <= enemy.EnemyCurrentData.navMeshAgent.stoppingDistance)
        //         {
        //             if (enemy.EnemyCurrentData.m_WaitTime <= 0)
        //             {
        //                 NextPoint();
        //                 Move(enemy.EnemyCurrentData.speedWalk);
        //                 enemy.EnemyCurrentData.m_WaitTime = Random.Range(0f, enemy.EnemyCurrentData.maxWaitTime);
        //             }
        //             else
        //             {
        //                 Stop();
        //                 enemy.EnemyCurrentData.m_WaitTime -= Time.deltaTime;
        //             }
        //         }
        // }
    }
}
