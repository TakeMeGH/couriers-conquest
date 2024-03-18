using CC.Characters;
using CC.Characters.States;
using UnityEngine;


namespace CC.Characters.States
{
    public class PlayerSprintingState : PlayerMovingState
    {
        private float startTime;

        private bool keepSprinting;
        private bool shouldResetSprintState;

        public PlayerSprintingState(PlayerControllerStatesMachine _playerController) : base(_playerController)
        {
        }

        public override void Enter()
        {
            _playerController.PlayerCurrentData.MovementSpeedModifier = _playerController.PlayerMovementData.SprintSpeedModifier;

            base.Enter();

            StartAnimation("isSprinting");

            // stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StrongForce;

            startTime = Time.time;

            shouldResetSprintState = true;

            if (!_playerController.PlayerCurrentData.ShouldSprint)
            {
                keepSprinting = false;
            }
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation("isSprinting");

            if (shouldResetSprintState)
            {
                keepSprinting = false;

                _playerController.PlayerCurrentData.ShouldSprint = false;
            }
        }

        public override void Update()
        {
            base.Update();

            if (keepSprinting)
            {
                return;
            }

            if (Time.time < startTime + _playerController.PlayerMovementData.SprintToRunTime)
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

        // protected override void OnJumpStarted(InputAction.CallbackContext context)
        // {
        //     shouldResetSprintState = false;

        //     base.OnJumpStarted(context);
        // }

        // protected override void OnFall()
        // {
        //     shouldResetSprintState = false;

        //     base.OnFall();
        // }


    }

}

