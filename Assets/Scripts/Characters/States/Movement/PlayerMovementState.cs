using CC.StateMachine;
using UnityEngine;

namespace CC.Characters.States
{
    public class PlayerMovementState : IState
    {
        protected PlayerControllerStatesMachine _playerController;
        public PlayerMovementState(PlayerControllerStatesMachine _playerController)
        {
            this._playerController = _playerController;
            InitializeData();

        }

        public virtual void Enter()
        {
            AddInputActions();
        }

        public virtual void Exit()
        {
            RemoveInputActions();
        }

        public virtual void Update()
        {
        }

        public virtual void PhysicsUpdate()
        {
            Move();
        }

        public virtual void OnTriggerEnter(Collider collider)
        {
            if (_playerController.LayerData.IsGroundLayer(collider.gameObject.layer))
            {
                OnContactWithGround(collider);

                return;
            }
        }

        public virtual void OnTriggerExit(Collider collider)
        {
            if (_playerController.LayerData.IsGroundLayer(collider.gameObject.layer))
            {
                OnContactWithGroundExited(collider);

                return;
            }
        }

        public virtual void OnAnimationEnterEvent()
        {
        }

        public virtual void OnAnimationExitEvent()
        {
        }

        public virtual void OnAnimationTransitionEvent()
        {
        }

        private void InitializeData()
        {
            SetBaseCameraRecenteringData();

            SetBaseRotationData();
        }

        protected void SetBaseCameraRecenteringData()
        {
        }

        protected void SetBaseRotationData()
        {
            _playerController.PlayerCurrentData.TargetRotationReachTime = _playerController.PlayerMovementData.GroundedTargetRotationReachTime;

            _playerController.PlayerCurrentData.TimeToReachTargetRotation = _playerController.PlayerCurrentData.TargetRotationReachTime;
        }

        // TODO: Ganti Animation Hash
        protected void StartAnimation(string transitionName)
        {
            _playerController.Animator.SetBool(transitionName, true);
        }

        protected void StopAnimation(string transitionName)
        {
            _playerController.Animator.SetBool(transitionName, false);
        }

        protected virtual void AddInputActions()
        {
            _playerController.InputReader.MovePerformed += OnMovementPerformed;
            _playerController.InputReader.MoveCanceled += OnMovementCanceled;
            _playerController.InputReader.MoveEvent += ReadMovementInput;

            _playerController.TriggerOnMovementStateAnimationEnterEvent.OnEventRaised += OnAnimationEnterEvent;
            _playerController.TriggerOnMovementStateAnimationExitEvent.OnEventRaised += OnAnimationExitEvent;
            _playerController.TriggerOnMovementStateAnimationTransitionEvent.OnEventRaised += OnAnimationTransitionEvent;

            _playerController.TriggerEnterEvent += OnTriggerEnter;
            _playerController.TriggerExitEvent += OnTriggerExit;
        }

        protected virtual void RemoveInputActions()
        {
            _playerController.InputReader.MovePerformed -= OnMovementPerformed;
            _playerController.InputReader.MoveCanceled -= OnMovementCanceled;
            _playerController.InputReader.MoveEvent -= ReadMovementInput;

            _playerController.TriggerOnMovementStateAnimationEnterEvent.OnEventRaised -= OnAnimationEnterEvent;
            _playerController.TriggerOnMovementStateAnimationExitEvent.OnEventRaised -= OnAnimationExitEvent;
            _playerController.TriggerOnMovementStateAnimationTransitionEvent.OnEventRaised -= OnAnimationTransitionEvent;

            _playerController.TriggerEnterEvent -= OnTriggerEnter;
            _playerController.TriggerExitEvent -= OnTriggerExit;
        }

        protected virtual void OnMovementPerformed(Vector2 movement)
        {
        }

        protected virtual void OnMovementCanceled()
        {
        }

        private void ReadMovementInput(Vector2 movement)
        {
            _playerController.PlayerCurrentData.MovementInput = movement;
        }

        private void Move()
        {
            if (_playerController.PlayerCurrentData.MovementInput == Vector2.zero || _playerController.PlayerCurrentData.MovementSpeedModifier == 0f)
            {
                return;
            }

            Vector3 movementDirection = GetMovementInputDirection();

            float targetRotationYAngle = Rotate(movementDirection);

            Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);

            float movementSpeed = GetMovementSpeed();

            Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();

            _playerController.Rigidbody.AddForce(targetRotationDirection * movementSpeed - currentPlayerHorizontalVelocity, ForceMode.VelocityChange);
        }

        protected Vector3 GetMovementInputDirection()
        {
            return new Vector3(_playerController.PlayerCurrentData.MovementInput.x, 0f, _playerController.PlayerCurrentData.MovementInput.y);
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
            float currentYAngle = _playerController.Rigidbody.rotation.eulerAngles.y;
            if (currentYAngle == _playerController.PlayerCurrentData.CurrentTargetRotation.y)
            {
                return;
            }

            float smoothedYAngle = Mathf.SmoothDampAngle(currentYAngle, _playerController.PlayerCurrentData.CurrentTargetRotation.y, ref _playerController.PlayerCurrentData.DampedTargetRotationCurrentVelocity.y, _playerController.PlayerCurrentData.TimeToReachTargetRotation.y - _playerController.PlayerCurrentData.DampedTargetRotationPassedTime.y);

            _playerController.PlayerCurrentData.DampedTargetRotationPassedTime.y += Time.deltaTime;

            Quaternion targetRotation = Quaternion.Euler(0f, smoothedYAngle, 0f);

            _playerController.Rigidbody.MoveRotation(targetRotation);
        }

        protected Vector3 GetTargetRotationDirection(float targetRotationAngle)
        {
            return Quaternion.Euler(0f, targetRotationAngle, 0f) * Vector3.forward;
        }

        protected float GetMovementSpeed(bool shouldConsiderSlopes = true)
        {
            float movementSpeed = _playerController.PlayerMovementData.GroundedBaseSpeed * _playerController.PlayerCurrentData.MovementSpeedModifier;

            if (shouldConsiderSlopes)
            {
                movementSpeed *= _playerController.PlayerCurrentData.MovementOnSlopesSpeedModifier;
            }

            return movementSpeed;
        }

        protected Vector3 GetPlayerHorizontalVelocity()
        {
            Vector3 playerHorizontalVelocity = _playerController.Rigidbody.velocity;

            playerHorizontalVelocity.y = 0f;

            return playerHorizontalVelocity;
        }

        protected Vector3 GetPlayerVerticalVelocity()
        {
            return new Vector3(0f, _playerController.Rigidbody.velocity.y, 0f);
        }

        protected virtual void OnContactWithGround(Collider collider)
        {
        }

        protected virtual void OnContactWithGroundExited(Collider collider)
        {
        }

        protected void ResetVelocity()
        {
            _playerController.Rigidbody.velocity = Vector3.zero;
        }

        protected void ResetVerticalVelocity()
        {
            Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();

            _playerController.Rigidbody.velocity = playerHorizontalVelocity;
        }

        protected void DecelerateHorizontally()
        {
            Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();

            _playerController.Rigidbody.AddForce(-playerHorizontalVelocity * _playerController.PlayerCurrentData.MovementDecelerationForce, ForceMode.Acceleration);
        }

        protected void DecelerateVertically()
        {
            Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();

            _playerController.Rigidbody.AddForce(-playerVerticalVelocity * _playerController.PlayerCurrentData.MovementDecelerationForce, ForceMode.Acceleration);
        }

        protected bool IsMovingHorizontally(float minimumMagnitude = 0.1f)
        {
            Vector3 playerHorizontaVelocity = GetPlayerHorizontalVelocity();

            Vector2 playerHorizontalMovement = new Vector2(playerHorizontaVelocity.x, playerHorizontaVelocity.z);

            return playerHorizontalMovement.magnitude > minimumMagnitude;
        }

        protected bool IsMovingUp(float minimumVelocity = 0.1f)
        {
            return GetPlayerVerticalVelocity().y > minimumVelocity;
        }

        protected bool IsMovingDown(float minimumVelocity = 0.1f)
        {
            return GetPlayerVerticalVelocity().y < -minimumVelocity;
        }
    }
}
