using UnityEngine;

namespace CC.Characters.States
{
    public class PlayerLightLandingState : PlayerLandingState
    {
        public PlayerLightLandingState(PlayerControllerStatesMachine _playerController) : base(_playerController)
        {
        }

        public override void Enter()
        {
            _playerController.PlayerCurrentData.MovementSpeedModifier = 0;

            base.Enter();

            _playerController.PlayerCurrentData.CurrentJumpForce = _playerController.PlayerMovementData.StationaryForce;

            ResetVelocity();
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

        public override void OnAnimationTransitionEvent()
        {
            _playerController.TransitionToState(PlayerControllerStatesMachine.PlayerStateEnum.IDLING);
        }
    }
}