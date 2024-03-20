using System.Collections;
using System.Collections.Generic;
using CC.Characters;
using CC.Characters.States;
using UnityEngine;

namespace CC.Characters.States
{
    public class PlayerGroundedState : PlayerMovementState
    {
        public PlayerGroundedState(PlayerControllerStatesMachine _playerController) : base(_playerController)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation("Grounded");

            UpdateShouldSprintState();

        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation("Grounded");
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            Float();
        }

        private void UpdateShouldSprintState()
        {
            if (!_playerController.PlayerCurrentData.ShouldSprint)
            {
                return;
            }

            if (_playerController.PlayerCurrentData.MovementInput != Vector2.zero)
            {
                return;
            }

            _playerController.PlayerCurrentData.ShouldSprint = false;
        }

        private void Float()
        {
            Vector3 capsuleColliderCenterInWorldSpace = _playerController.ResizableCapsuleCollider.CapsuleColliderData.Collider.bounds.center;

            Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

            if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, _playerController.ResizableCapsuleCollider.SlopeData.FloatRayDistance, _playerController.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
            {
                float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

                float slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);

                if (slopeSpeedModifier == 0f)
                {
                    return;
                }

                float distanceToFloatingPoint = _playerController.ResizableCapsuleCollider.CapsuleColliderData.ColliderCenterInLocalSpace.y * _playerController.transform.localScale.y - hit.distance;

                if (distanceToFloatingPoint == 0f)
                {
                    return;
                }

                float amountToLift = distanceToFloatingPoint * _playerController.ResizableCapsuleCollider.SlopeData.StepReachForce - GetPlayerVerticalVelocity().y;

                Vector3 liftForce = new Vector3(0f, amountToLift, 0f);

                _playerController.Rigidbody.AddForce(liftForce, ForceMode.VelocityChange);
            }
        }

        private float SetSlopeSpeedModifierOnAngle(float angle)
        {
            float slopeSpeedModifier = _playerController.PlayerMovementData.SlopeSpeedAngles.Evaluate(angle);

            if (_playerController.PlayerCurrentData.MovementOnSlopesSpeedModifier != slopeSpeedModifier)
            {
                _playerController.PlayerCurrentData.MovementOnSlopesSpeedModifier = slopeSpeedModifier;

            }

            return slopeSpeedModifier;
        }

        protected override void AddInputActions()
        {
            base.AddInputActions();

            _playerController.InputReader.DashPerformed += OnDashStarted;

            _playerController.InputReader.JumpPerformed += OnJumpStarted;
        }

        protected override void RemoveInputActions()
        {
            base.RemoveInputActions();

            _playerController.InputReader.DashPerformed -= OnDashStarted;

            _playerController.InputReader.JumpPerformed -= OnJumpStarted;
        }

        protected virtual void OnDashStarted()
        {
            _playerController.TransitionToState(PlayerControllerStatesMachine.PlayerStateEnum.DASHING);
        }

        protected virtual void OnJumpStarted()
        {
            _playerController.TransitionToState(PlayerControllerStatesMachine.PlayerStateEnum.JUMPING);
        }

        protected virtual void OnMove()
        {
            if (_playerController.PlayerCurrentData.ShouldSprint)
            {
                _playerController.TransitionToState(PlayerControllerStatesMachine.PlayerStateEnum.SPRINTING);

                return;
            }

            _playerController.TransitionToState(PlayerControllerStatesMachine.PlayerStateEnum.RUNING);
        }

        protected override void OnContactWithGroundExited(Collider collider)
        {
            if (IsThereGroundUnderneath())
            {
                return;
            }

            Vector3 capsuleColliderCenterInWorldSpace = _playerController.ResizableCapsuleCollider.CapsuleColliderData.Collider.bounds.center;

            Ray downwardsRayFromCapsuleBottom = new Ray(capsuleColliderCenterInWorldSpace - _playerController.ResizableCapsuleCollider.CapsuleColliderData.ColliderVerticalExtents, Vector3.down);

            if (!Physics.Raycast(downwardsRayFromCapsuleBottom, out _, _playerController.PlayerMovementData.GroundToFallRayDistance, _playerController.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
            {
                OnFall();
            }
        }

        private bool IsThereGroundUnderneath()
        {
            DataBlueprint.PlayerTriggerColliderData triggerColliderData = _playerController.ResizableCapsuleCollider.TriggerColliderData;

            Vector3 groundColliderCenterInWorldSpace = triggerColliderData.GroundCheckCollider.bounds.center;

            Collider[] overlappedGroundColliders = Physics.OverlapBox(groundColliderCenterInWorldSpace, triggerColliderData.GroundCheckColliderVerticalExtents, triggerColliderData.GroundCheckCollider.transform.rotation, _playerController.LayerData.GroundLayer, QueryTriggerInteraction.Ignore);

            return overlappedGroundColliders.Length > 0;
        }

        protected virtual void OnFall()
        {
            _playerController.TransitionToState(PlayerControllerStatesMachine.PlayerStateEnum.FALLING);
        }

        protected override void OnMovementPerformed(Vector2 movement)
        {
            base.OnMovementPerformed(movement);

            UpdateTargetRotation(GetMovementInputDirection());
        }
    }

}
