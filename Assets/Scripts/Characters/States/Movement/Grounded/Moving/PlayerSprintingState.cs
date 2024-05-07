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
        private StaminaController staminaController;

        public PlayerSprintingState(PlayerControllerStatesMachine _playerController, StaminaController staminaController) : base(_playerController)
        {
            this.staminaController = staminaController;
        }

        public override void Enter()
        {
            _playerController.PlayerCurrentData.MovementSpeedModifier = _playerController.PlayerMovementData.SprintSpeedModifier;

            base.Enter();

            StartAnimation("isSprinting");

            _playerController.PlayerCurrentData.CurrentJumpForce = _playerController.PlayerMovementData.MediumForce;

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

            // Cek apa stamina lebih besar dari 0
            if (_playerController.GetComponent<StaminaController>().GetCurrentStamina() <= 0)
            {
                // Kalau stamina ga lebih besar dari 0, stop nge sprint
                StopSprinting();
                return;
            }
            else if (keepSprinting)
            {
                float sprintStaminaCost = _playerController.PlayerMovementData.SprintStaminaCost;
                staminaController.DecreaseStaminaByAmount(sprintStaminaCost * Time.deltaTime); // Stamina ngurang per detik berdasar poin yang udah di set di PlayerMovementSO saat sprinting
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
                _playerController.SwitchState(_playerController.PlayerIdlingState);
                return;
            }

            _playerController.SwitchState(_playerController.PlayerRuningState);
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
            _playerController.SwitchState(_playerController.PlayerMediumStoppingState);
            base.OnMovementCanceled();
        }

        protected override void OnJumpStarted()
        {
            shouldResetSprintState = false;

            base.OnJumpStarted();
        }

        protected override void OnFall()
        {
            shouldResetSprintState = false;

            base.OnFall();
        }
    }
}
