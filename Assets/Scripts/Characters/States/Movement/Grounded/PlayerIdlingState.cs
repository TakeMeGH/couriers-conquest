using CC.Characters;
using CC.Characters.States;
using Unity.VisualScripting;
using UnityEngine;

namespace CC.Characters.States
{
    public class PlayerIdlingState : PlayerGroundedState
    {
        bool _alreadyCalled;

        public PlayerIdlingState(PlayerControllerStatesMachine _playerController) : base(_playerController)
        {
        }

        public override void Enter()
        {
            _alreadyCalled = false;

            _playerController.PlayerCurrentData.MovementSpeedModifier = 0f;

            _playerController.PlayerCurrentData.CurrentJumpForce = _playerController.PlayerMovementData.StationaryForce;

            ResetVelocity();

            base.Enter();

            StartAnimation("isIdling");

        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation("isIdling");

            EnableRigidbody();

        }

        public override void Update()
        {
            if (_playerController.PlayerCurrentData.IsUpdateNewTransform)
            {
                return;
            }

            base.Update();

            if (_playerController.PlayerCurrentData.MovementInput == Vector2.zero)
            {
                return;
            }

            OnMove();
        }

        public override void PhysicsUpdate()
        {
            if (_playerController.PlayerCurrentData.IsUpdateNewTransform)
            {
                return;
            }

            base.PhysicsUpdate();

            if (!IsMovingHorizontally())
            {
                return;
            }

            ResetVelocity();
        }

        public override void OnAnimationEnterEvent()
        {
            base.OnAnimationEnterEvent();
            if (_playerController.PlayerCurrentData.IsUpdateNewTransform)
            {
                _playerController.transform.position = _playerController.PlayerCurrentData.NewTransformPosition;
                _playerController.PlayerCurrentData.IsUpdateNewTransform = false;
            }

        }
        public override void OnAnimationTransitionEvent()
        {
            if (_alreadyCalled) return;
            _alreadyCalled = true;

            EnableRigidbody();
        }
    }
}
