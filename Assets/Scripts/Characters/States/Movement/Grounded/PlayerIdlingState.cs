using CQ.Characters;
using CQ.Characters.States;
using UnityEngine;

public class PlayerIdlingState : PlayerGroundedState
{
    public PlayerIdlingState(PlayerControllerStatesMachine _playerController) : base(_playerController)
    {
    }

    public override void Enter()
    {
        base.Enter();

        StartAnimation("isIdling");
        ResetVelocity();
        _playerController.PlayerCurrentData.SpeedModifier = 0f;
    }

    public override void Update()
    {
        base.Update();

        if (_playerController.PlayerCurrentData.MovementInput != Vector2.zero)
        {
            _playerController.TransitionToState(PlayerControllerStatesMachine.PlayerStateEnum.RUNING);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }


    public override void Exit()
    {
        base.Exit();
        StopAnimation("isIdling");

    }

}
