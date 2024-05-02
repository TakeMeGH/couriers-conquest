using System.Collections;
using System.Collections.Generic;
using CC.Enemy;
using CC.StateMachine;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace CC.Enemy.States
{
public class EnemyState : IState
{
    protected Enemy enemy;

    public EnemyState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public virtual void Enter()
    {
        
    }

    public virtual void Exit()
    {
        
    }

    public virtual void Update()
    {
    }

    public virtual void PhysicsUpdate()
    {
    }

    public virtual void OnAnimationEnterEvent()
    {
    }

    public virtual void OnAnimationExitEvent()
    {
    }

    public virtual void OnAnimationTransitionEvent()
    {
    }
}
}
