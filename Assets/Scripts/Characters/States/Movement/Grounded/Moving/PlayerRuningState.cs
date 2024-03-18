using CQ.Characters;
using UnityEngine;

public class PlayerRuningState : PlayerMovingState
{
    public PlayerRuningState(PlayerControllerStatesMachine _playerController) : base(_playerController)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation("isRuning");
        _playerController.PlayerCurrentData.SpeedModifier = 1f;

    }

    public override void Update()
    {
        base.Update();
        if (_playerController.PlayerCurrentData.MovementInput == Vector2.zero)
        {
            _playerController.TransitionToState(PlayerControllerStatesMachine.PlayerStateEnum.IDLING);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation("isRuning");
    }
    protected override void OnMovementCanceled()
    {
        _playerController.TransitionToState(PlayerControllerStatesMachine.PlayerStateEnum.IDLING);
        base.OnMovementCanceled();
    }

}
