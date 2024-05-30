using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Characters.States
{
    public class PlayerClimbUpState : PlayerClimbingState
    {
        Vector3 _newTransformPosition;

        public PlayerClimbUpState(PlayerControllerStatesMachine _playerController) : base(_playerController)
        {
        }

        public override void Enter()
        {
            base.Enter();
            StartAnimation("isClimbingUp");
        }
        public override void Update()
        {
            base.Update();

        }

        public override void Exit()
        {

            StopAnimation("isClimbingUp");
            base.Exit();

        }

        public override void OnAnimationTransitionEvent()
        {
            _newTransformPosition = _playerController.transform.position;
            _newTransformPosition += _playerController.transform.forward * _playerController.OffsetStandUpPoint.z;
            _newTransformPosition += _playerController.transform.up * _playerController.OffsetStandUpPoint.y;
            _playerController.PlayerCurrentData.NewTransformPosition = _newTransformPosition;
            _playerController.PlayerCurrentData.IsUpdateNewTransform = true;
            _playerController.SwitchState(_playerController.PlayerIdlingState);

        }
    }
}