namespace CC.Characters.States
{
    public class PlayerBlockState : PlayerGroundedState
    {
        public PlayerBlockState(PlayerControllerStatesMachine _playerController) : base(_playerController)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation("Block");
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation("Block");
        }
    }
}