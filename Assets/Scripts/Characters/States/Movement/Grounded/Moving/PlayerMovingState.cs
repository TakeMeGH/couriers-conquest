using CQ.Characters;
using CQ.Characters.States;

public class PlayerMovingState : PlayerGroundedState
{
    public PlayerMovingState(PlayerControllerStatesMachine _playerController) : base(_playerController)
    {
    }

}
