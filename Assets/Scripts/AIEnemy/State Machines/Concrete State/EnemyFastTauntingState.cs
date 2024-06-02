using UnityEngine;

namespace CC.Enemy.States
{
    public class EnemyFastTauntingState : EnemyControllerState
    {
        public EnemyFastTauntingState(EnemyController enemy) : base(enemy)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation("isFastTaunting");
            
            _enemyController.HealthBar.gameObject.SetActive(true);
            LookAt(_enemyController.EnemyCurrentData.PlayerTransform);

            _enemyController.NavMeshAgent.speed = 0;
            _enemyController.NavMeshAgent.velocity = Vector3.zero;
            _enemyController.NavMeshAgent.isStopped = true;


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

            StopAnimation("isFastTaunting");
        }

        public override void OnAnimationExitEvent()
        {
            _enemyController.SwitchState(_enemyController.IdleAttackState);
        }

    }
}