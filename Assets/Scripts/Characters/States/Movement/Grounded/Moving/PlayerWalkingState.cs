using UnityEngine;

namespace CC.Characters.States
{
    public class PlayerWalkingState : PlayerMovingState
    {
        public PlayerWalkingState(PlayerControllerStatesMachine _playerController) : base(_playerController)
        {
        }

        public override void Enter()
        {
            _playerController.PlayerCurrentData.MovementSpeedModifier = _playerController.PlayerMovementData.WalkSpeedModifier;

            base.Enter();

            StartAnimation("isWalking");

            _playerController.PlayerCurrentData.CurrentJumpForce = _playerController.PlayerMovementData.MediumForce;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation("isWalking");

            SetBaseCameraRecenteringData();
        }
        protected override void OnWalkToggleStarted()
        {
            base.OnWalkToggleStarted();

            _playerController.SwitchState(_playerController.PlayerRuningState);
        }

        protected override void OnMovementCanceled()
        {
            _playerController.SwitchState(_playerController.PlayerMediumStoppingState);

            base.OnMovementCanceled();
        }
    }
}