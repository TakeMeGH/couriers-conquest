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

            int randomValue = Random.Range(0, 2);
            _enemyController.EnemyCurrentData.IsHeavyAttack = randomValue;

            StartAnimation("isAttacking");
            _enemyController.Animator.SetFloat("isHeavyAttack", _enemyController.EnemyCurrentData.IsHeavyAttack);

            _enemyController.WeaponDamage.SetAttack();

            _enemyController.NavMeshAgent.speed = 0;
            _enemyController.NavMeshAgent.isStopped = true;

            _enemyController.transform.LookAt(_enemyController.EnemyCurrentData.PlayerTransform.position);
        }


        public override void Update()
        {
            base.Update();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation("isAttacking");

        }
        public override void OnAnimationExitEvent()
        {
            base.OnAnimationExitEvent();

            float distance = Vector3.Distance(_enemyController.transform.position,
    _enemyController.EnemyCurrentData.PlayerTransform.transform.position);

            if (distance < _enemyController.EnemyPersistenceData.TooCloseDistance)
            {
                _enemyController.SwitchState(_enemyController.StepBackState);
                return;
            }

            _enemyController.SwitchState(_enemyController.ChasingState);
        }


    }
}
