using CC.Characters;
using CC.Characters.States;

public class PlayerMovingState : PlayerGroundedState
{
    public PlayerMovingState(PlayerControllerStatesMachine _playerController) : base(_playerController)
    {
    }

    public override void Enter()
    {
        base.Enter();

        StartAnimation("Moving");
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation("Moving");
    }


}
