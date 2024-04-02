using UnityEngine;

namespace CC.Characters.States
{
    public class PlayerJumpingState : PlayerAirborneState
    {
        private bool shouldKeepRotating;
        private bool canStartFalling;

        public PlayerJumpingState(PlayerControllerStatesMachine _playerController) : base(_playerController)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _playerController.PlayerCurrentData.MovementSpeedModifier = 0f;

            _playerController.PlayerCurrentData.MovementDecelerationForce = _playerController.PlayerMovementData.DecelerationForce;

            _playerController.PlayerCurrentData.TargetRotationReachTime = _playerController.PlayerMovementData.JumpTargetRotationReachTime;

            shouldKeepRotating = _playerController.PlayerCurrentData.MovementInput != Vector2.zero;

            Jump();
        }

        public override void Exit()
        {
            base.Exit();

            SetBaseRotationData();

            canStartFalling = false;
        }

        public override void Update()
        {
            base.Update();

            if (!canStartFalling && IsMovingUp(0f))
            {
                canStartFalling = true;
            }

            if (!canStartFalling || IsMovingUp(0f))
            {
                return;
            }

            _playerController.SwitchState(_playerController.PlayerFallingState);
            // _playerController.TransitionToState(PlayerControllerStatesMachine.PlayerStateEnum.FALLING);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (shouldKeepRotating)
            {
                RotateTowardsTargetRotation();
            }

            if (IsMovingUp())
            {
                DecelerateVertically();
            }
        }

        private void Jump()
        {
            Vector3 jumpForce = _playerController.PlayerCurrentData.CurrentJumpForce;

            Vector3 jumpDirection = _playerController.transform.forward;

            if (shouldKeepRotating)
            {
                UpdateTargetRotation(GetMovementInputDirection());

                jumpDirection = GetTargetRotationDirection(_playerController.PlayerCurrentData.CurrentTargetRotation.y);
            }

            jumpForce.x *= jumpDirection.x;
            jumpForce.z *= jumpDirection.z;

            jumpForce = GetJumpForceOnSlope(jumpForce);

            ResetVelocity();

            _playerController.Rigidbody.AddForce(jumpForce, ForceMode.VelocityChange);
        }

        private Vector3 GetJumpForceOnSlope(Vector3 jumpForce)
        {
            Vector3 capsuleColliderCenterInWorldSpace = _playerController.ResizableCapsuleCollider.CapsuleColliderData.Collider.bounds.center;

            Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

            if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, _playerController.PlayerMovementData.JumpToGroundRayDistance, _playerController.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
            {
                float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

                if (IsMovingUp())
                {
                    float forceModifier = _playerController.PlayerMovementData.JumpForceModifierOnSlopeUpwards.Evaluate(groundAngle);

                    jumpForce.x *= forceModifier;
                    jumpForce.z *= forceModifier;
                }

                if (IsMovingDown())
                {
                    float forceModifier = _playerController.PlayerMovementData.JumpForceModifierOnSlopeDownwards.Evaluate(groundAngle);

                    jumpForce.y *= forceModifier;
                }
            }

            return jumpForce;
        }

        protected override void ResetSprintState()
        {
        }

        protected override void OnMovementCanceled()
        {
        }
    }
}