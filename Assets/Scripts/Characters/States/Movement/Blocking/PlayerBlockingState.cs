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
            _playerController.Health.SetInvunerable(true);
            //_playerController.Animator.CrossFadeInFixedTime(BlockHash, CrossFadeDuration);
            base.Enter();
            StartAnimation(blocking.AnimationName);
        }

        public override void Exit()
        {
            //_playerController.Health.SetInvunerable(false);
            base.Exit();
            _playerController.Health.SetInvunerable(false);
            StartAnimation(blocking.AnimationName);
        }

        public override void Update()
        {
            base.Update();

            if (!_playerController.InputReader.IsBlocking)
            {
                _playerController.SwitchState(new PlayerBlockState(_playerController));
                return;
            }
            //if (_playerController.Targeter.CurrentTarget == null)
            //{
               // _playerController.SwitchState(new PlayerGroundedState(_playerController));
               // return;
           // }
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
