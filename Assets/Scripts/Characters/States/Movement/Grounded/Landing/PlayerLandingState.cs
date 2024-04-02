namespace CC.Characters.States
{
    public class PlayerLandingState : PlayerGroundedState
    {
        public PlayerLandingState(PlayerControllerStatesMachine _playerController) : base(_playerController)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation("Landing");
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation("Landing");
        }
    }
}