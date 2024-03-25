namespace CC.Characters.States
{
    public class PlayerMediumStoppingState : PlayerStoppingState
    {
        public PlayerMediumStoppingState(PlayerControllerStatesMachine _playerController) : base(_playerController)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _playerController.PlayerCurrentData.MovementDecelerationForce = _playerController.PlayerMovementData.MediumDecelerationForce;

            _playerController.PlayerCurrentData.CurrentJumpForce = _playerController.PlayerMovementData.MediumForce;
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}