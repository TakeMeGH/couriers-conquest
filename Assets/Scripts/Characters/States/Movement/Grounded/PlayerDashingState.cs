using CQ.Characters;
using UnityEngine;

public class PlayerDashingState : PlayerGroundedState
{
    public PlayerDashingState(PlayerControllerStatesMachine _playerController) : base(_playerController)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation("isDashing");
        _playerController.PlayerCurrentData.SpeedModifier = 2f;

    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation("isDashing");

    }

}
