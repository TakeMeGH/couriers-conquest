using CC.Characters;
using CC.Characters.States;
using Unity.VisualScripting;
using UnityEngine;

namespace CC.Characters.States
{
    public class PlayerIdlingState : PlayerGroundedState
    {
        public PlayerIdlingState(PlayerControllerStatesMachine _playerController) : base(_playerController)
        {
        }

        public override void Enter()
        {
            _playerController.PlayerCurrentData.MovementSpeedModifier = 0f;

            // stateMachine.ReusableData.BackwardsCameraRecenteringData = groundedData.IdleData.BackwardsCameraRecenteringData;

            base.Enter();

            StartAnimation("isIdling");

            _playerController.PlayerCurrentData.CurrentJumpForce = _playerController.PlayerMovementData.StationaryForce;

            ResetVelocity();
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation("isIdling");
        }

        public override void Update()
        {
            base.Update();

            if (_playerController.PlayerCurrentData.MovementInput == Vector2.zero)
            {
                return;
            }

            OnMove();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (!IsMovingHorizontally())
            {
                return;
            }

            ResetVelocity();
        }
    }
}
