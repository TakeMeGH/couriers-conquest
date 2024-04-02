using UnityEngine;


namespace CC.Characters.States
{
    public class PlayerAttackingState : PlayerAttackState
    {
        private float previousFrameTime;
        private bool alreadyAppliedForce;

        private AttackSO attack;
        public PlayerAttackingState(PlayerControllerStatesMachine _playerController, int attackIndex) : base(_playerController)
        {
            attack = _playerController.Attacks[attackIndex];
        }

        public override void Enter()
        {
            base.Enter();
            _playerController.Weapon.SetAttack(attack.Damage);
            StartAnimation(attack.AnimationName);

            // stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
            
            ResetVelocity();
            alreadyAppliedForce = false;
            previousFrameTime = 0;
        }

        public override void Update()
        {
            // base.Update();

            float normalizedTime = GetNormalizedTime();

            if (normalizedTime >= previousFrameTime && normalizedTime < 1f)
            {
                if (normalizedTime >= attack.ForceTime)
                {
                    TryApplyForce();
                }

                if (_playerController.InputReader.IsAttacking)
                {
                    TryComboAttack(normalizedTime);
                }
            }
            else if (normalizedTime >= 1f)
            {
                _playerController.SwitchState(_playerController.PlayerIdlingState);
                // if (stateMachine.Targeter.CurrentTarget != null)
                // {
                //     stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
                // }
                // else
                // {
                //     stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
                // }
            }
            previousFrameTime = normalizedTime;
        }

        public override void PhysicsUpdate()
        {
            Float();
        }

        public override void Exit()
        {
            base.Exit();
            previousFrameTime = 0f;
            StopAnimation(attack.AnimationName);

        }

        private void TryComboAttack(float normalizedTime)
        {
            if (attack.ComboStateIndex == -1) { return; }

            if (normalizedTime < attack.ComboAttackTime) { return; }

            _playerController.SwitchState(_playerController.PlayerAttackingStates[attack.ComboStateIndex]);
        }

        private void TryApplyForce()
        {
            if (alreadyAppliedForce) { return; }

            _playerController.Rigidbody.AddForce(_playerController.transform.forward * attack.Force, ForceMode.Acceleration);

            alreadyAppliedForce = true;
        }

        private float GetNormalizedTime()
        {
            AnimatorStateInfo currentInfo = _playerController.Animator.GetCurrentAnimatorStateInfo(0);
            AnimatorStateInfo nextInfo = _playerController.Animator.GetNextAnimatorStateInfo(0);

            if (_playerController.Animator.IsInTransition(0) && nextInfo.IsTag("Attack"))
            {
                return nextInfo.normalizedTime;
            }
            else if (!_playerController.Animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
            {
                return currentInfo.normalizedTime;
            }
            else
            {
                return 0f;
            }
        }
    }
}