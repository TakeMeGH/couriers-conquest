using CC.Characters;
using UnityEngine;


namespace CC.Characters.States
{

    public class PlayerRuningState : PlayerMovingState
    {
        // private float startTime;
        public PlayerRuningState(PlayerControllerStatesMachine _playerController) : base(_playerController)
        {
        }

        public override void Enter()
        {
            _playerController.PlayerCurrentData.MovementSpeedModifier = _playerController.PlayerMovementData.RunSpeedModifier;

            base.Enter();

            StartAnimation("isRuning");

            // stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.MediumForce;

            // startTime = Time.time;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation("isRuning");
        }

        public override void Update()
        {
            base.Update();

            StopRunning();
        }

        private void StopRunning()
        {
            if (_playerController.PlayerCurrentData.MovementInput == Vector2.zero)
            {
                _playerController.TransitionToState(PlayerControllerStatesMachine.PlayerStateEnum.IDLING);
                return;
            }

        }

        protected override void OnMovementCanceled()
        {
            _playerController.TransitionToState(PlayerControllerStatesMachine.PlayerStateEnum.IDLING);

            base.OnMovementCanceled();
        }
    }

}

