using System.Collections;
using System.Collections.Generic;
using CC.Enemy;
using UnityEngine;

namespace CC.Enemy.States
{
    public class EnemyAttackState : EnemyControllerState
    {
        public EnemyAttackState(EnemyController EnemyControllerState) : base(EnemyControllerState)
        {
        }

        public override void Enter()
        {
            base.Enter();


            if (_enemyController.EnemyCurrentData.IsHeavyAttack)
            {
                StartAnimation("isHeavyAttack");
            }
            else
            {
                StartAnimation("isLightAttack");
            }

            _enemyController.WeaponDamage.SetAttack();

            _enemyController.NavMeshAgent.speed = 0;
            _enemyController.NavMeshAgent.velocity = Vector3.zero;
            _enemyController.NavMeshAgent.isStopped = true;

            _enemyController.transform.LookAt(_enemyController.EnemyCurrentData.PlayerTransform.position);
        }


        public override void Update()
        {
            base.Update();

            _enemyController.NavMeshAgent.velocity = Vector3.zero;
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void Exit()
        {
            base.Exit();

            if (_enemyController.EnemyCurrentData.IsHeavyAttack)
            {
                StopAnimation("isHeavyAttack");
            }
            else
            {
                StopAnimation("isLightAttack");
            }

        }
        public override void OnAnimationExitEvent()
        {
            base.OnAnimationExitEvent();

            _enemyController.SwitchState(_enemyController.ChasingState);
        }
    }
}
