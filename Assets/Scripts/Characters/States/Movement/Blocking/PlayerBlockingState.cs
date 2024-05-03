using UnityEngine;

namespace CC.Characters.States
{
    public class PlayerBlockingState : PlayerBlockState
    {
        private BlockSO blocking;
        //private readonly int BlockHash = Animator.StringToHash("Block");
        //private const float CrossFadeDuration = 0.1f;
        private StaminaController staminaController;

        public PlayerBlockingState(PlayerControllerStatesMachine _playerController, StaminaController staminaController) : base(_playerController)
        {
            blocking = _playerController.Block;
            this.staminaController = staminaController;
        }

        public override void Enter()
    {
        if (!staminaController.CanBlock)
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

        if (!_playerController.InputReader.IsBlocking || !staminaController.CanBlock)
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

