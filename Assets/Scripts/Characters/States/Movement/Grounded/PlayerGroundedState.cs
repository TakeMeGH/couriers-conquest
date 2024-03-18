using System.Collections;
using System.Collections.Generic;
using CQ.Characters;
using CQ.Characters.States;
using UnityEngine;

public class PlayerGroundedState : PlayerMovementState
{
    public PlayerGroundedState(PlayerControllerStatesMachine _playerController) : base(_playerController)
    {
    }

}
