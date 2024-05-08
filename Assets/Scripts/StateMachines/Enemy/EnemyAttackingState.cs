/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.StateMachine.Enemy
public class EnemyAttackingState : EnemyBaseState
{
    private readonly int AttackHash = Animator.StringToHash("Attack");

    private const float TransitionDuration = 0.1f;

    public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Weapon.SetAttack(stateMachine.AttackDamage);

        stateMachine.Animator.CrossFadeInFixedTime(AttackHash, TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {

    }

    public override void Exit() { }
}
*/
