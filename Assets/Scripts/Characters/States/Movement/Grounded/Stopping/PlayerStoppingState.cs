using UnityEngine;

namespace CC.Characters.States
{
    public class PlayerStoppingState : PlayerGroundedState
    {
        public PlayerStoppingState(PlayerControllerStatesMachine _playerController) : base(_playerController)
        {
        }

        public override void Enter()
        {
            _playerController.PlayerCurrentData.MovementSpeedModifier = 0f;

            SetBaseCameraRecenteringData();

            base.Enter();

            StartAnimation("Stopping");
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation("Stopping");
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            RotateTowardsTargetRotation();

            if (!IsMovingHorizontally())
            {
                return;
            }

            DecelerateHorizontally();
        }

        public override void OnAnimationTransitionEvent()
        {
            _playerController.TransitionToState(PlayerControllerStatesMachine.PlayerStateEnum.IDLING);
        }

        protected override void AddInputActions()
        {
            base.AddInputActions();

            _playerController.InputReader.MoveEvent += OnMovementStarted;
        }

        protected override void RemoveInputActions()
        {
            base.RemoveInputActions();

            _playerController.InputReader.MoveEvent -= OnMovementStarted;
        }

        private void OnMovementStarted(Vector2 movement)
        {
            OnMove();
        }
    }
}