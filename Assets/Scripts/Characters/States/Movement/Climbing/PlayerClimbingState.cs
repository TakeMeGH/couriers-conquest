using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Characters.States
{
    public class PlayerClimbingState : PlayerMovementState
    {
        public PlayerClimbingState(PlayerControllerStatesMachine _playerController) : base(_playerController)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation("Climbing");
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation("Climbing");
        }


    }
}
