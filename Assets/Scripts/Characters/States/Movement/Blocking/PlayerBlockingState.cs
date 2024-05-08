using UnityEngine;

namespace CC.Characters.States
{
    public class PlayerBlockingState : PlayerBlockState
    {
        private BlockSO blocking;
        //private readonly int BlockHash = Animator.StringToHash("Block");
        //private const float CrossFadeDuration = 0.1f;
        public PlayerBlockingState(PlayerControllerStatesMachine _playerController) : base(_playerController)
        {
            blocking = _playerController.Block;
        }

        public override void Enter()
        {
            if (!_playerController.StaminaController.CanBlock)
            {
                _playerController.SwitchState(_playerController.PlayerIdlingState);
                return;
            }

            _playerController.Health.SetBlocking(true);
            base.Enter();
            ResetVelocity();

            StartAnimation(blocking.AnimationName);
        }

        public override void Exit()
        {
            base.Exit();
            _playerController.Health.SetBlocking(false);
            StopAnimation(blocking.AnimationName);
        }

        public override void Update()
        {
            base.Update();

            if (!_playerController.InputReader.IsBlocking || !_playerController.StaminaController.CanBlock)
            {
                _playerController.SwitchState(_playerController.PlayerIdlingState);
                return;
            }
        }

        public override void PhysicsUpdate()
        {
            Float();
        }
    }
}

