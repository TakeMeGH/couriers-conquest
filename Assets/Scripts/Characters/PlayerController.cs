using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.FiniteStateMachine;

namespace CQ.Characters
{
    public class PlayerController : AbstractFiniteStateMachine
    {
        [SerializeField] InputReader _inputReader;
        Animator _animator;
        Rigidbody _rigidBody;
        protected Vector2 _movementInput;
        protected float _baseSpeed = 5f;
        protected float _speedmodifier = 1f;
        private Vector3 _currentTargetRotation;
        private Vector3 _timeToReachTargetRotation;
        private Vector3 _dampedTargetRotationCurrentVelocity;
        private Vector3 _dampedTargetRotationPassedTime;
        private Transform _mainCameraTransform;


        public enum PlayerStateEnum
        {
            IDLING,
            RUNING,
            DASHING
        }
        /*
        Notes : Jangan gunakan ready apabila menggunakan state machine. 
        Hal ini menyebabkan OnEnter pada state default tidak terpanggil.
        */
        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _mainCameraTransform = UnityEngine.Camera.main.transform;
            _animator = GetComponentInChildren<Animator>();
            Init(PlayerStateEnum.IDLING,
                AbstractState.Create<IdlingState, PlayerStateEnum>(PlayerStateEnum.IDLING, this),
                AbstractState.Create<RuningState, PlayerStateEnum>(PlayerStateEnum.RUNING, this),
                AbstractState.Create<DashingState, PlayerStateEnum>(PlayerStateEnum.DASHING, this)

            );

            _timeToReachTargetRotation.y = 0.14f;
        }
        private void OnEnable()
        {
            _inputReader.MoveEvent += OnMovementPerformed;
        }

        private void OnDisable()
        {
            _inputReader.MoveEvent -= OnMovementPerformed;
        }

        public class IdlingState : AbstractState
        {
            PlayerController _playerController;
            public override void OnEnter()
            {
                _playerController = GetStateMachine<PlayerController>();
                _playerController.StartAnimation("isIdling");
                _playerController.ResetVelocity();
                _playerController._speedmodifier = 0f;

            }
            public override void OnUpdate()
            {
                _playerController = GetStateMachine<PlayerController>();
                if (_playerController._movementInput != Vector2.zero)
                {
                    TransitionToState(PlayerStateEnum.RUNING);
                }
            }
            public override void OnExit()
            {
                _playerController.StopAnimation("isIdling");

            }
        }
        public class RuningState : AbstractState
        {
            PlayerController _playerController;
            public override void OnEnter()
            {
                _playerController = GetStateMachine<PlayerController>();
                _playerController.StartAnimation("isRuning");
                _playerController._speedmodifier = 1f;


            }
            public override void OnUpdate()
            {
                if (_playerController._movementInput == Vector2.zero)
                {
                    TransitionToState(PlayerStateEnum.IDLING);
                }
            }

            public override void OnFixedUpdate()
            {
                _playerController.Move();
            }
            public override void OnExit()
            {
                _playerController.StopAnimation("isRuning");

            }
        }

        public class DashingState : AbstractState
        {
            PlayerController _playerController;
            public override void OnEnter()
            {
                _playerController = GetStateMachine<PlayerController>();
                _playerController._speedmodifier = 2f;


            }
            public override void OnUpdate()
            {
            }

            public override void OnExit()
            {
            }
        }

        protected void StartAnimation(string animationName)
        {
            _animator.SetBool(animationName, true);
        }

        protected void StopAnimation(string animationName)
        {
            _animator.SetBool(animationName, false);
        }


        protected void ResetVelocity()
        {
            _rigidBody.velocity = Vector3.zero;
        }

        protected void OnMovementPerformed(Vector2 movement)
        {
            _movementInput = movement;
        }

        private void Move()
        {
            if (_movementInput == Vector2.zero || _speedmodifier == 0f)
            {
                return;
            }


            Vector3 movementDirection = GetMovementInputDirection();

            float targetRotationYAngle = Rotate(movementDirection);

            Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);

            float movementSpeed = GetMovementSpeed();

            Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();

            _rigidBody.AddForce(targetRotationDirection * movementSpeed - currentPlayerHorizontalVelocity, ForceMode.VelocityChange);
        }

        protected float GetMovementSpeed(bool shouldConsiderSlopes = true)
        {
            return _baseSpeed * _speedmodifier;
        }


        protected Vector3 GetMovementInputDirection()
        {
            return new Vector3(_movementInput.x, 0f, _movementInput.y);
        }

        protected Vector3 GetPlayerHorizontalVelocity()
        {
            Vector3 playerHorizontalVelocity = _rigidBody.velocity;

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


            if (directionAngle != _currentTargetRotation.y)
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
            angle += _mainCameraTransform.eulerAngles.y;

            if (angle > 360f)
            {
                angle -= 360f;
            }

            return angle;
        }


        private void UpdateTargetRotationData(float targetAngle)
        {
            _currentTargetRotation.y = targetAngle;

            _dampedTargetRotationPassedTime.y = 0f;
        }

        protected void RotateTowardsTargetRotation()
        {
            float currentYAngle = _rigidBody.rotation.eulerAngles.y;

            if (currentYAngle == _currentTargetRotation.y)
            {
                return;
            }

            float smoothedYAngle = Mathf.SmoothDampAngle(currentYAngle, _currentTargetRotation.y, ref _dampedTargetRotationCurrentVelocity.y, _timeToReachTargetRotation.y - _dampedTargetRotationPassedTime.y);

            _dampedTargetRotationPassedTime.y += Time.deltaTime;

            Quaternion targetRotation = Quaternion.Euler(0f, smoothedYAngle, 0f);

            _rigidBody.MoveRotation(targetRotation);
        }

        protected Vector3 GetTargetRotationDirection(float targetRotationAngle)
        {
            return Quaternion.Euler(0f, targetRotationAngle, 0f) * Vector3.forward;
        }




    }
}
