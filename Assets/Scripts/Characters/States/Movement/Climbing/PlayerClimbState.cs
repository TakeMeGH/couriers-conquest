using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace CC.Characters.States
{
    public class PlayerClimbState : PlayerClimbingState
    {
        public PlayerClimbState(PlayerControllerStatesMachine _playerController) : base(_playerController)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _playerController.InputReader.DropClimbingPerformed += OnDrop;
            _playerController.FreeClimb.OnAboveStandAble += OnAboveStandAble;
            _playerController.FreeClimb.OnMCMove += OnClimbMove;
        }
        public override void Update()
        {
            base.Update();

            _playerController.FreeClimb.Tick(Time.deltaTime);
        }

        public override void Exit()
        {
            base.Exit();

            _playerController.InputReader.DropClimbingPerformed -= OnDrop;
            _playerController.FreeClimb.OnAboveStandAble -= OnAboveStandAble;
            _playerController.FreeClimb.OnMCMove -= OnClimbMove;


            _playerController.FreeClimb.Rig.weight = 0;
        }

        void OnDrop()
        {
            _playerController.PlayerCurrentData.CurrentDropTime = _playerController.PlayerCurrentData.MaxDropTime;

            _playerController.SwitchState(_playerController.PlayerFallingState);
        }

        void OnAboveStandAble()
        {
            _playerController.SwitchState(_playerController.PlayerClimbUpState);

        }

        void OnClimbMove()
        {
            if (_playerController.StaminaController.GetCurrentStamina() <= 0)
            {
                OnDrop();
                return;
            }

            _playerController.StaminaController.DecreaseStaminaByAmount(
                    _playerController.PlayerMovementData.ClimbStaminaCost);
        }

    }
}
