using System.Collections;
using System.Collections.Generic;
using CC.Enemy;
using UnityEngine;

namespace CC.Enemy.States
{
    public class EnemyTauntState : EnemyControllerState
    {

        int _tauntCount = 0;
        public EnemyTauntState(EnemyController enemy) : base(enemy)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation("isTaunting");

            _tauntCount = 0;

            _enemyController.NavMeshAgent.speed = 0;
            _enemyController.NavMeshAgent.velocity = Vector3.zero;
            _enemyController.NavMeshAgent.isStopped = true;

            _enemyController.transform.LookAt(_enemyController.EnemyCurrentData.PlayerTransform.position);
            _enemyController.EnemyCurrentData.IsPlayerInRange = false;
        }


        public override void Update()
        {
            base.Update();

            _enemyController.NavMeshAgent.velocity = Vector3.zero;


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

            StopAnimation("isTaunting");
        }

        public override void OnAnimationExitEvent()
        {
            base.OnAnimationExitEvent();

            _tauntCount++;

            if (_tauntCount == _enemyController.EnemyPersistenceData.TauntCount) _enemyController.SwitchState(_enemyController.PatrolingState);

        }

    }
}