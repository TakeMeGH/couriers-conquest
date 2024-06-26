using UnityEngine;

namespace CC.Characters.States
{
    public class PlayerAirborneState : PlayerMovementState
    {
        public PlayerAirborneState(PlayerControllerStatesMachine _playerController) : base(_playerController)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation("Airborne");

            ResetSprintState();
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation("Airborne");
        }

        protected virtual void ResetSprintState()
        {
            _playerController.PlayerCurrentData.ShouldSprint = false;
        }

        protected override void OnContactWithGround(Collider collider)
        {
            _playerController.SwitchState(_playerController.PlayerLightLandingState);
            // _playerController.TransitionToState(PlayerControllerStatesMachine.PlayerStateEnum.LIGHTLANDING);
        }

        public void CheckForClimb()
        {
            if (_playerController.FreeClimb.CheckForClimb())
            {
                _playerController.SwitchState(_playerController.PlayerClimbState);

            }

        }
    }
}