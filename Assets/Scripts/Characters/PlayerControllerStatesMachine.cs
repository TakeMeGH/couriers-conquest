using UnityEngine;
using KevinCastejon.FiniteStateMachine;
using QC.Characters;

namespace CQ.Characters
{
    public class PlayerControllerStatesMachine : AbstractFiniteStateMachine
    {
        [field: SerializeField] public InputReader InputReader { get; private set; }
        [field: SerializeField] public PlayerMovementSO PlayerMovementData { get; private set; }

        public Animator Animator { get; private set; }
        public Rigidbody RigidBody { get; private set; }
        public Transform MainCameraTransform { get; private set; }
        public PlayerStateData PlayerCurrentData { get; private set; }

        #region State
        private PlayerIdlingState _playerIdlingState;
        private PlayerRuningState _playerRuningState;
        private PlayerDashingState _playerDashingState;
        private PlayerSprintingState _playerSprintingState;

        #endregion


        public enum PlayerStateEnum
        {
            IDLING,
            RUNING,
            DASHING,
            SPRINTING,
        }
        /*
        Notes : Jangan gunakan ready apabila menggunakan state machine. 
        Hal ini menyebabkan OnEnter pada state default tidak terpanggil.
        */
        private void Awake()
        {
            Init(PlayerStateEnum.IDLING,
                AbstractState.Create<IdlingState, PlayerStateEnum>(PlayerStateEnum.IDLING, this),
                AbstractState.Create<RuningState, PlayerStateEnum>(PlayerStateEnum.RUNING, this),
                AbstractState.Create<DashingState, PlayerStateEnum>(PlayerStateEnum.DASHING, this),
                AbstractState.Create<SprintingState, PlayerStateEnum>(PlayerStateEnum.SPRINTING, this)

            );
            Initialize();
        }

        private void Initialize()
        {
            PlayerCurrentData = new PlayerStateData();
            RigidBody = GetComponent<Rigidbody>();
            MainCameraTransform = UnityEngine.Camera.main.transform;
            Animator = GetComponentInChildren<Animator>();
            PlayerCurrentData.TimeToReachTargetRotation = PlayerMovementData.TargetRotationReachTime;

            _playerIdlingState = new PlayerIdlingState(this);
            _playerRuningState = new PlayerRuningState(this);
            _playerSprintingState = new PlayerSprintingState(this);
        }
        public class IdlingState : AbstractState
        {
            PlayerControllerStatesMachine _playerController;
            public override void OnEnter()
            {
                _playerController = GetStateMachine<PlayerControllerStatesMachine>();
                _playerController._playerIdlingState.Enter();


            }
            public override void OnUpdate()
            {

                _playerController._playerIdlingState.Update();

            }
            public override void OnFixedUpdate()
            {
                _playerController._playerIdlingState.PhysicsUpdate();

            }

            public override void OnExit()
            {

                _playerController._playerIdlingState.Exit();


            }
        }
        public class RuningState : AbstractState
        {
            PlayerControllerStatesMachine _playerController;
            public override void OnEnter()
            {
                _playerController = GetStateMachine<PlayerControllerStatesMachine>();
                _playerController._playerRuningState.Enter();

            }
            public override void OnUpdate()
            {
                _playerController._playerRuningState.Update();

            }

            public override void OnFixedUpdate()
            {
                _playerController._playerRuningState.PhysicsUpdate();

            }
            public override void OnExit()
            {
                _playerController._playerRuningState.Exit();


            }
        }

        public class DashingState : AbstractState
        {
            PlayerControllerStatesMachine _playerController;
            public override void OnEnter()
            {
                _playerController = GetStateMachine<PlayerControllerStatesMachine>();
                _playerController._playerDashingState.Enter();
            }
            public override void OnUpdate()
            {
                _playerController._playerDashingState.Update();

            }

            public override void OnFixedUpdate()
            {
                _playerController._playerDashingState.PhysicsUpdate();
            }

            public override void OnExit()
            {
                _playerController._playerDashingState.Exit();

            }
        }
        public class SprintingState : AbstractState
        {
            PlayerControllerStatesMachine _playerController;
            public override void OnEnter()
            {
                _playerController = GetStateMachine<PlayerControllerStatesMachine>();
                _playerController._playerSprintingState.Enter();
            }
            public override void OnUpdate()
            {
                _playerController._playerSprintingState.Update();

            }

            public override void OnFixedUpdate()
            {
                _playerController._playerSprintingState.PhysicsUpdate();
            }

            public override void OnExit()
            {
                _playerController._playerSprintingState.Exit();

            }
        }

    }
}
