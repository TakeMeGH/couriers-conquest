using System.Collections;
using System.Collections.Generic;
using CC.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace CC.Enemy.States
{
    public class EnemyIdleAttackState : EnemyControllerState
    {

        float idleTime = 0;
        float currentIdleTime = 0;
        public EnemyIdleAttackState(EnemyController enemy) : base(enemy)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation("isIdleAttack");

            _enemyController.NavMeshAgent.speed = 0;
            _enemyController.NavMeshAgent.velocity = Vector3.zero;
            _enemyController.NavMeshAgent.isStopped = true;
            
            currentIdleTime = 0;

            idleTime = Random.Range(_enemyController.EnemyPersistenceData.MinIdleTime, _enemyController.EnemyPersistenceData.MaxIdleTime);
        }


        public override void Update()
        {
            base.Update();

            currentIdleTime += Time.deltaTime;
            if (currentIdleTime >= idleTime)
            {
                // TODO : RANDOM ATTACK
                _enemyController.EnemyCurrentData.IsHeavyAttack = true;
                _enemyController.SwitchState(_enemyController.FastChasingState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation("isIdleAttack");

        }

    }
}