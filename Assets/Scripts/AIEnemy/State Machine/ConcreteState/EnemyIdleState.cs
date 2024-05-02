using System.Collections;
using System.Collections.Generic;
using CC.Enemy;
using UnityEngine;

namespace CC.Enemy.States
{
public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(Enemy enemy) : base(enemy)
    {
    }

        public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void OnAnimationEnterEvent()
    {
        base.OnAnimationEnterEvent();
    }
}
}
