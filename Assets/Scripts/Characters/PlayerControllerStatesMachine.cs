using UnityEngine;
using KevinCastejon.FiniteStateMachine;
using CC.Characters;

namespace CC.Characters
{
    public class PlayerControllerStatesMachine : AbstractFiniteStateMachine
    {
        [field: SerializeField] public InputReader InputReader { get; private set; }
        [field: SerializeField] public PlayerMovementSO PlayerMovementData { get; private set; }
        [field: SerializeField] public Events.VoidEventChannelSO TriggerOnMovementStateAnimationEnterEvent { get; private set; }
        [field: SerializeField] public Events.VoidEventChannelSO TriggerOnMovementStateAnimationExitEvent { get; private set; }
        [field: SerializeField] public Events.VoidEventChannelSO TriggerOnMovementStateAnimationTransitionEvent { get; private set; }

        public Animator Animator { get; private set; }
        public Rigidbody Rigidbody { get; private set; }
        public Transform MainCameraTransform { get; private set; }
        public PlayerStateData PlayerCurrentData { get; private set; }

        #region State
        private States.PlayerIdlingState _playerIdlingState;
        private States.PlayerRuningState _playerRuningState;
        private States.PlayerDashingState _playerDashingState;
        private States.PlayerSprintingState _playerSprintingState;

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
            Rigidbody = GetComponent<Rigidbody>();
            MainCameraTransform = UnityEngine.Camera.main.transform;
            Animator = GetComponentInChildren<Animator>();
            PlayerCurrentData.TimeToReachTargetRotation = PlayerMovementData.TargetRotationReachTime;

            _playerIdlingState = new States.PlayerIdlingState(this);
            _playerRuningState = new States.PlayerRuningState(this);
            _playerDashingState = new States.PlayerDashingState(this);
            _playerSprintingState = new States.PlayerSprintingState(this);
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
