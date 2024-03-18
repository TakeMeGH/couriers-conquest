using CQ.StatesInteface;
using UnityEngine;

namespace CQ.Characters.States
{
    public class PlayerMovementState : IState
    {
        protected PlayerControllerStatesMachine _playerController;
        public PlayerMovementState(PlayerControllerStatesMachine _playerController)
        {
            this._playerController = _playerController;
        }

        public virtual void Enter()
        {
            AddInputActions();
        }

        public virtual void Exit()
        {
        }

        public virtual void PhysicsUpdate()
        {
            Move();
        }

        public virtual void Update()
        {
        }

        protected virtual void AddInputActions()
        {
            _playerController.InputReader.MoveEvent += OnMovementPerformed;
        }


        protected virtual void RemoveInputActions()
        {
            _playerController.InputReader.MoveEvent -= OnMovementPerformed;

        }


        public void StartAnimation(string animationName)
        {
            _playerController.Animator.SetBool(animationName, true);
        }

        public void StopAnimation(string animationName)
        {
            _playerController.Animator.SetBool(animationName, false);
        }


        public void ResetVelocity()
        {
            _playerController.RigidBody.velocity = Vector3.zero;
        }

        public void OnMovementPerformed(Vector2 movement)
        {
            _playerController.PlayerCurrentData.MovementInput = movement;
        }

        public void Move()
        {
            if (_playerController.PlayerCurrentData.MovementInput == Vector2.zero ||
                _playerController.PlayerCurrentData.SpeedModifier == 0f)
            {
                return;
            }


            Vector3 movementDirection = GetMovementInputDirection();

            float targetRotationYAngle = Rotate(movementDirection);

            Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);

            float movementSpeed = GetMovementSpeed();

            Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();

            _playerController.RigidBody.AddForce(targetRotationDirection * movementSpeed - currentPlayerHorizontalVelocity, ForceMode.VelocityChange);
        }

        protected float GetMovementSpeed(bool shouldConsiderSlopes = true)
        {
            return _playerController.PlayerMovementData.BaseSpeed * _playerController.PlayerCurrentData.SpeedModifier;
        }


        protected Vector3 GetMovementInputDirection()
        {
            return new Vector3(_playerController.PlayerCurrentData.MovementInput.x, 0f, _playerController.PlayerCurrentData.MovementInput.y);
        }

        protected Vector3 GetPlayerHorizontalVelocity()
        {
            Vector3 playerHorizontalVelocity = _playerController.RigidBody.velocity;

            playerHorizontalVelocity.y = 0f;

            return playerHorizontalVelocity;
        }

        private float Rotate(Vector3 direction)
        {
            float directionAngle = UpdateTargetRotation(direction);

            RotateTowardsTargetRotation();

            return directionAngle;
        }

        protected float UpdateTargetRotation(Vector3 direction, bool shouldConsiderCameraRotation = true)
        {
            float directionAngle = GetDirectionAngle(direction);

            if (shouldConsiderCameraRotation)
            {
                directionAngle = AddCameraRotationToAngle(directionAngle);
            }


            if (directionAngle != _playerController.PlayerCurrentData.CurrentTargetRotation.y)
            {
                UpdateTargetRotationData(directionAngle);
            }

            return directionAngle;
        }

        private float GetDirectionAngle(Vector3 direction)
        {
            float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            if (directionAngle < 0f)
            {
                directionAngle += 360f;
            }

            return directionAngle;
        }

        private float AddCameraRotationToAngle(float angle)
        {
            angle += _playerController.MainCameraTransform.eulerAngles.y;

            if (angle > 360f)
            {
                angle -= 360f;
            }

            return angle;
        }


        private void UpdateTargetRotationData(float targetAngle)
        {
            _playerController.PlayerCurrentData.CurrentTargetRotation.y = targetAngle;

            _playerController.PlayerCurrentData.DampedTargetRotationPassedTime.y = 0f;
        }

        protected void RotateTowardsTargetRotation()
        {
            float currentYAngle = _playerController.RigidBody.rotation.eulerAngles.y;

            if (currentYAngle == _playerController.PlayerCurrentData.CurrentTargetRotation.y)
            {
                return;
            }

            float smoothedYAngle = Mathf.SmoothDampAngle(currentYAngle, _playerController.PlayerCurrentData.CurrentTargetRotation.y,
                ref _playerController.PlayerCurrentData.DampedTargetRotationCurrentVelocity.y,
                _playerController.PlayerCurrentData.TimeToReachTargetRotation.y - _playerController.PlayerCurrentData.DampedTargetRotationPassedTime.y);

            _playerController.PlayerCurrentData.DampedTargetRotationPassedTime.y += Time.deltaTime;

            Quaternion targetRotation = Quaternion.Euler(0f, smoothedYAngle, 0f);

            _playerController.RigidBody.MoveRotation(targetRotation);
        }

        protected Vector3 GetTargetRotationDirection(float targetRotationAngle)
        {
            return Quaternion.Euler(0f, targetRotationAngle, 0f) * Vector3.forward;
        }
    }

}
