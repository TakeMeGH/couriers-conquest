using UnityEngine;

namespace CC.Enemy.States
{
    public class EnemyIdleState : EnemyControllerState
    {
        public EnemyIdleState(EnemyController enemy) : base(enemy)
        {
        }

        // public override void Enter()
        // {
        //     base.Enter();
        //     if (ShouldStartChasing())
        //     {
        //         Debug.Log("Chasing");
        //         enemy.SwitchState(enemy.ChasingState);
        //     }
        //     else if (ShouldStartPatroling())
        //     {
        //         Debug.Log("Patroling");
        //         enemy.SwitchState(enemy.PatrolingState);
        //     }
        // }

        // public override void Exit()
        // {
        //     base.Exit();
        // }

        // public override void Update()
        // {
        //     base.Update();
        // }

        // public override void PhysicsUpdate()
        // {
        //     base.PhysicsUpdate();
        // }

        // public override void OnAnimationEnterEvent()
        // {
        //     base.OnAnimationEnterEvent();
        // }

        // private bool ShouldStartChasing()
        // {
        //     return enemy.EnemyCurrentData.m_PlayerInRange;
        // }

        // private bool ShouldStartPatroling()
        // {
        //     return !enemy.EnemyCurrentData.m_PlayerInRange;
        // }
    }
}
