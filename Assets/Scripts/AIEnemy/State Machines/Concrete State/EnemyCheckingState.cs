using UnityEngine;
using System;
using System.Diagnostics;


namespace CC.Enemy.States
{
    public class EnemyCheckingState : EnemyControllerState
    {
        public EnemyCheckingState(EnemyController _enemyController) : base(_enemyController)
        {
        }

        public override void Enter()
        {
            base.Enter();
            StartAnimation("isChecking");

            _enemyController.NavMeshAgent.speed = 0;
            _enemyController.NavMeshAgent.isStopped = true;

        }

        public override void Update()
        {
            base.Update();

            if (_enemyController.EnemyCurrentData.IsPlayerInRange)
            {
                _enemyController.SwitchState(_enemyController.ChasingState);
            }

        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation("isChecking");
        }
        public override void OnAnimationExitEvent()
        {
            _enemyController.SwitchState(_enemyController.PatrolingState);
        }
    }
}