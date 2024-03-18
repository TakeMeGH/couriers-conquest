using CQ.Characters;
using CQ.Characters.States;
using UnityEngine;

public class PlayerSprintingState : PlayerMovingState
{
    private float startTime;

    private bool keepSprinting;
    // private bool shouldResetSprintState;

    public PlayerSprintingState(PlayerControllerStatesMachine _playerController) : base(_playerController)
    {
    }

    public override void Enter()
    {
        _playerController.PlayerCurrentData.SpeedModifier = 1.7f;

        base.Enter();

        StartAnimation("isSprint");

        startTime = Time.time;

        // shouldResetSprintState = true;

        // if (!stateMachine.ReusableData.ShouldSprint)
        // {
        //     keepSprinting = false;
        // }
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation("isSprint");
        keepSprinting = false;

        // if (shouldResetSprintState)
        // {
        //     keepSprinting = false;

        //     stateMachine.ReusableData.ShouldSprint = false;
        // }
    }

    public override void Update()
    {
        base.Update();

        if (keepSprinting)
        {
            return;
        }

        if (Time.time < startTime + 1f)
        {
            return;
        }

        StopSprinting();
    }

    private void StopSprinting()
    {
        if (_playerController.PlayerCurrentData.MovementInput == Vector2.zero)
        {
            _playerController.TransitionToState(PlayerControllerStatesMachine.PlayerStateEnum.IDLING);

            return;
        }

        _playerController.TransitionToState(PlayerControllerStatesMachine.PlayerStateEnum.RUNING);
    }

    protected override void AddInputActions()
    {
        base.AddInputActions();

        _playerController.InputReader.StartedSprinting += OnSprintPerformed;
    }

    protected override void RemoveInputActions()
    {
        base.RemoveInputActions();

        _playerController.InputReader.StartedSprinting -= OnSprintPerformed;
    }

    private void OnSprintPerformed()
    {
        keepSprinting = true;

        _playerController.PlayerCurrentData.ShouldSprint = true;
    }

    protected override void OnMovementCanceled()
    {
        _playerController.TransitionToState(PlayerControllerStatesMachine.PlayerStateEnum.IDLING);
        base.OnMovementCanceled();
    }


}
