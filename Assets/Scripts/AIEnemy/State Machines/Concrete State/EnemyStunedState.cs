
using UnityEngine;

namespace CC.Enemy.States
{
    public class EnemyStunedState : EnemyControllerState
    {
        public EnemyStunedState(EnemyController enemy) : base(enemy)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _enemyController.EnemyCurrentData.AttackedCount = 0;
            _enemyController.Animator.SetTrigger("Stuned");
            
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
        }
        public override void OnAnimationExitEvent()
        {
            _enemyController.SwitchState(_enemyController.IdleAttackState);
        }



    }

}