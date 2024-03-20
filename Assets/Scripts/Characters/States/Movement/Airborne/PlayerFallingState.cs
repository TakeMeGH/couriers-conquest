using UnityEngine;

namespace CC.Characters.States
{
    public class PlayerFallingState : PlayerAirborneState
    {
        private Vector3 playerPositionOnEnter;

        public PlayerFallingState(PlayerControllerStatesMachine _playerController) : base(_playerController)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation("isFalling");

            _playerController.PlayerCurrentData.MovementSpeedModifier = 0f;

            playerPositionOnEnter = _playerController.transform.position;

            ResetVerticalVelocity();
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation("isFalling");
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            LimitVerticalVelocity();
        }

        private void LimitVerticalVelocity()
        {
            Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();

            if (playerVerticalVelocity.y >= -_playerController.PlayerMovementData.FallSpeedLimit)
            {
                return;
            }

            Vector3 limitedVelocityForce = new Vector3(0f, -_playerController.PlayerMovementData.FallSpeedLimit - playerVerticalVelocity.y, 0f);

            _playerController.Rigidbody.AddForce(limitedVelocityForce, ForceMode.VelocityChange);
        }

        protected override void ResetSprintState()
        {
        }

        protected override void OnContactWithGround(Collider collider)
        {
            _playerController.TransitionToState(PlayerControllerStatesMachine.PlayerStateEnum.LIGHTLANDING);
        }
    }
}