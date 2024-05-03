using UnityEditor;
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

            ResetRotationXZ();
            // _playerController.transform.rotation = Quaternion.Euler(0, _playerController.transform.rotation.eulerAngles.y, 0);
            // Debug.Log(_playerController.transform.eulerAngles + " ROTATION");
            playerPositionOnEnter = _playerController.transform.position;

            ResetVerticalVelocity();
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation("isFalling");
            _playerController.PlayerCurrentData.CurrentDropTime = 0f;

        }

        public override void Update()
        {
            base.Update();
            ResetRotationXZ();

            // Debug.Log(_playerController.transform.eulerAngles + " ROTATION");

            if (_playerController.PlayerCurrentData.CurrentDropTime <= 0f)
            {
                if (_playerController.FreeClimb.CheckForClimb())
                {
                    _playerController.Rigidbody.useGravity = false;
                    _playerController.Rigidbody.isKinematic = true;
                    _playerController.SwitchState(_playerController.PlayerClimbState);

                }
            }
            else
            {
                _playerController.PlayerCurrentData.CurrentDropTime -= Time.deltaTime;
            }
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

            _playerController.Rigidbody.AddForce(limitedVelocityForce, ForceMode.Impulse);
        }

        protected override void ResetSprintState()
        {
        }

        protected override void OnContactWithGround(Collider collider)
        {
            _playerController.SwitchState(_playerController.PlayerLightLandingState);
            // _playerController.TransitionToState(PlayerControllerStatesMachine.PlayerStateEnum.LIGHTLANDING);
        }

        void ResetRotationXZ()
        {
            _playerController.transform.rotation = Quaternion.Euler(0, _playerController.transform.rotation.eulerAngles.y, 0);

        }
    }
}