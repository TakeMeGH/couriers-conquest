namespace CC.Characters.States
{
    public class PlayerAttackState : PlayerGroundedState
    {
        public PlayerAttackState(PlayerControllerStatesMachine _playerController) : base(_playerController)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation("Attack");
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation("Attack");
        }
    }
}