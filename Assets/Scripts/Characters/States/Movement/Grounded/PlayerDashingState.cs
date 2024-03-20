using CC.Characters;
using UnityEngine;

namespace CC.Characters.States
{
    public class PlayerDashingState : PlayerGroundedState
    {
        private float startTime;

        private int consecutiveDashesUsed;

        private bool shouldKeepRotating;

        public PlayerDashingState(PlayerControllerStatesMachine _playerController) : base(_playerController)
        {
        }

        public override void Enter()
        {
            _playerController.PlayerCurrentData.MovementSpeedModifier = _playerController.PlayerMovementData.DashSpeedModifier;

            base.Enter();

            StartAnimation("isDashing");

            _playerController.PlayerCurrentData.CurrentJumpForce = _playerController.PlayerMovementData.MediumForce;

            _playerController.PlayerCurrentData.TargetRotationReachTime = _playerController.PlayerMovementData.DashTargetRotationReachTime;

            Dash();

            shouldKeepRotating = _playerController.PlayerCurrentData.MovementInput != Vector2.zero;

            startTime = Time.time;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation("isDashing");

            SetBaseRotationData();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (!shouldKeepRotating)
            {
                return;
            }

            RotateTowardsTargetRotation();
        }

        public override void OnAnimationTransitionEvent()
        {
            if (_playerController.PlayerCurrentData.MovementInput == Vector2.zero)
            {
                _playerController.TransitionToState(PlayerControllerStatesMachine.PlayerStateEnum.MEDIUMSTOPPING);

                return;
            }

            _playerController.TransitionToState(PlayerControllerStatesMachine.PlayerStateEnum.SPRINTING);
        }

        protected override void AddInputActions()
        {
            base.AddInputActions();

            _playerController.InputReader.MovePerformed += OnMovementPerformed;

        }

        protected override void RemoveInputActions()
        {
            base.RemoveInputActions();

            _playerController.InputReader.MovePerformed -= OnMovementPerformed;
        }

        protected override void OnMovementPerformed(Vector2 movement)
        {
            base.OnMovementPerformed(movement);

            shouldKeepRotating = true;
        }

        private void Dash()
        {
            Vector3 dashDirection = _playerController.transform.forward;

            dashDirection.y = 0f;

            UpdateTargetRotation(dashDirection, false);

            if (_playerController.PlayerCurrentData.MovementInput != Vector2.zero)
            {
                UpdateTargetRotation(GetMovementInputDirection());

                dashDirection = GetTargetRotationDirection(_playerController.PlayerCurrentData.CurrentTargetRotation.y);
            }

            _playerController.Rigidbody.velocity = dashDirection * GetMovementSpeed(false);
        }
        protected override void OnDashStarted()
        {
        }

    }

}

