using CC.Characters;
using UnityEngine;


namespace CC.Characters.States
{

    public class PlayerRuningState : PlayerMovingState
    {
        public PlayerRuningState(PlayerControllerStatesMachine _playerController) : base(_playerController)
        {
        }

        public override void Enter()
        {
            _playerController.PlayerCurrentData.MovementSpeedModifier = _playerController.PlayerMovementData.RunSpeedModifier;

            base.Enter();

            StartAnimation("isRuning");

            _playerController.PlayerCurrentData.CurrentJumpForce = _playerController.PlayerMovementData.MediumForce;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation("isRuning");
        }

        public override void Update()
        {
            base.Update();

            if (!_playerController.PlayerCurrentData.ShouldWalk)
            {
                return;
            }

            StopRunning();
        }

        private void StopRunning()
        {
            if (_playerController.PlayerCurrentData.MovementInput == Vector2.zero)
            {
                _playerController.SwitchState(_playerController.PlayerIdlingState);
                return;
            }
            _playerController.SwitchState(_playerController.PlayerWalkingState);


        }

        protected override void OnWalkToggleStarted()
        {
            base.OnWalkToggleStarted();

            _playerController.SwitchState(_playerController.PlayerWalkingState);
        }


        protected override void OnMovementCanceled()
        {
            _playerController.SwitchState(_playerController.PlayerMediumStoppingState);
            // _playerController.TransitionToState(PlayerControllerStatesMachine.PlayerStateEnum.MEDIUMSTOPPING);
            base.OnMovementCanceled();
        }
    }

}

