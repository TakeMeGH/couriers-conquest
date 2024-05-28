using System;
using Cinemachine.Utility;
using Unity.VisualScripting;
using UnityEngine;


namespace CC.Characters.States
{
    public class PlayerAttackingState : PlayerAttackState
    {
        float _currentTimeToRotate = 0;
        float _timeToRotate = 0.2f;
        bool _isFullyRotated = false;
        bool _noNearestEnemy = false;
        float searchRadius = 3.0f;

        private float previousFrameTime;
        private bool alreadyAppliedForce;
        Vector3 _nearestEnemy;
        private AttackSO attack;
        public PlayerAttackingState(PlayerControllerStatesMachine _playerController, int attackIndex) : base(_playerController)
        {
            attack = _playerController.Attacks[attackIndex];
        }

        public override void Enter()
        {
            base.Enter();

            _playerController.Weapon.SetAttack();

            StartAnimation(attack.AnimationName);

            ResetVelocity();

            _noNearestEnemy = false;

            _nearestEnemy = GetNearestEnemy();

            _currentTimeToRotate = 0;
            _isFullyRotated = false;

            alreadyAppliedForce = false;
            previousFrameTime = 0;

        }

        public override void Update()
        {
            // base.Update();
            RotatePlayer();

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

        Vector3 GetNearestEnemy()
        {
            Vector3 position = _playerController.transform.position;

            Collider[] enemyColliders = Physics.OverlapSphere(position, searchRadius, 1 << 13);
            float closestDistance = Mathf.Infinity;
            Vector3 _nearestEnemy = _playerController.transform.position;

            _noNearestEnemy = true;
            foreach (var enemyColider in enemyColliders)
            {
                float distance = (enemyColider.transform.position - position).sqrMagnitude;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    _nearestEnemy = enemyColider.transform.position;
                    _noNearestEnemy = false;
                }
            }

            _nearestEnemy = new Vector3(_nearestEnemy.x, _playerController.transform.position.y,
                _nearestEnemy.z);

            return _nearestEnemy;
        }

        void RotatePlayer()
        {
            if (_isFullyRotated || _noNearestEnemy) return;
            // Draw();
            _currentTimeToRotate += Time.deltaTime;
            Vector3 direction = (_nearestEnemy - _playerController.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            _playerController.transform.rotation = Quaternion.Slerp(_playerController.transform.rotation, lookRotation, Math.Min(_currentTimeToRotate / _timeToRotate, 1));

            if (_currentTimeToRotate >= _timeToRotate) _isFullyRotated = true;
        }

        // void Draw()
        // {
        //     for (int i = 0; i < 360; i += 20)
        //     {
        //         Vector3 dir = Quaternion.Euler(0, i, 0) * Vector3.forward;
        //         Debug.DrawRay(_playerController.transform.position, dir * searchRadius, Color.red);
        //     }
        // }

    }
}
