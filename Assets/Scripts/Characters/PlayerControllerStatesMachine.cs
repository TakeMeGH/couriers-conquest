using UnityEngine;
using KevinCastejon.FiniteStateMachine;
using CC.Characters.DataBlueprint.Layers;
using UnityEngine.Events;

namespace CC.Characters
{
    public class PlayerControllerStatesMachine : AbstractFiniteStateMachine
    {
        [field: Header("Input Reader")]
        [field: SerializeField] public InputReader InputReader { get; private set; }

        [field: Header("Player Movement Data")]
        [field: SerializeField] public PlayerMovementSO PlayerMovementData { get; private set; }

        [field: Header("Animation Events")]
        [field: SerializeField] public Events.VoidEventChannelSO TriggerOnMovementStateAnimationEnterEvent { get; private set; }
        [field: SerializeField] public Events.VoidEventChannelSO TriggerOnMovementStateAnimationExitEvent { get; private set; }
        [field: SerializeField] public Events.VoidEventChannelSO TriggerOnMovementStateAnimationTransitionEvent { get; private set; }

        [field: Header("Collisions")]
        [field: SerializeField] public PlayerLayerData LayerData { get; private set; }

        #region Component
        public Animator Animator { get; private set; }
        public Rigidbody Rigidbody { get; private set; }
        public Transform MainCameraTransform { get; private set; }
        public PlayerStateData PlayerCurrentData { get; private set; }
        public PlayerResizableCapsuleCollider ResizableCapsuleCollider { get; private set; }
        #endregion

        #region Events
        public event UnityAction<Collider> TriggerEnterEvent = delegate { };
        public event UnityAction<Collider> TriggerExitEvent = delegate { };
        #endregion

        #region State
        private States.PlayerIdlingState _playerIdlingState;
        private States.PlayerRuningState _playerRuningState;
        private States.PlayerDashingState _playerDashingState;
        private States.PlayerSprintingState _playerSprintingState;
        private States.PlayerJumpingState _playerJumpingState;
        private States.PlayerFallingState _playerFallingState;
        private States.PlayerLightLandingState _playerLightLandingState;
        private States.PlayerMediumStoppingState _playerMediumStoppingState;


        #endregion


        public enum PlayerStateEnum
        {
            IDLING,
            RUNING,
            DASHING,
            SPRINTING,
            JUMPING,
            FALLING,
            LIGHTLANDING,
            MEDIUMSTOPPING,
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
                AbstractState.Create<SprintingState, PlayerStateEnum>(PlayerStateEnum.SPRINTING, this),
                AbstractState.Create<JumpingState, PlayerStateEnum>(PlayerStateEnum.JUMPING, this),
                AbstractState.Create<FallingState, PlayerStateEnum>(PlayerStateEnum.FALLING, this),
                AbstractState.Create<LightLandingState, PlayerStateEnum>(PlayerStateEnum.LIGHTLANDING, this),
                AbstractState.Create<MediumStoppingState, PlayerStateEnum>(PlayerStateEnum.MEDIUMSTOPPING, this)

            );
            Initialize();
        }

        private void Initialize()
        {
            Rigidbody = GetComponent<Rigidbody>();
            ResizableCapsuleCollider = GetComponent<PlayerResizableCapsuleCollider>();
            Animator = GetComponentInChildren<Animator>();

            PlayerCurrentData = new PlayerStateData();

            MainCameraTransform = UnityEngine.Camera.main.transform;
            PlayerCurrentData.TimeToReachTargetRotation = PlayerMovementData.TargetRotationReachTime;

            _playerIdlingState = new States.PlayerIdlingState(this);
            _playerRuningState = new States.PlayerRuningState(this);
            _playerDashingState = new States.PlayerDashingState(this);
            _playerSprintingState = new States.PlayerSprintingState(this);
            _playerJumpingState = new States.PlayerJumpingState(this);
            _playerFallingState = new States.PlayerFallingState(this);
            _playerLightLandingState = new States.PlayerLightLandingState(this);
            _playerMediumStoppingState = new States.PlayerMediumStoppingState(this);
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

        public class JumpingState : AbstractState
        {
            PlayerControllerStatesMachine _playerController;
            public override void OnEnter()
            {
                _playerController = GetStateMachine<PlayerControllerStatesMachine>();
                _playerController._playerJumpingState.Enter();
            }
            public override void OnUpdate()
            {
                _playerController._playerJumpingState.Update();

            }

            public override void OnFixedUpdate()
            {
                _playerController._playerJumpingState.PhysicsUpdate();
            }

            public override void OnExit()
            {
                _playerController._playerJumpingState.Exit();

            }
        }

        public class FallingState : AbstractState
        {
            PlayerControllerStatesMachine _playerController;
            public override void OnEnter()
            {
                _playerController = GetStateMachine<PlayerControllerStatesMachine>();
                _playerController._playerFallingState.Enter();
            }
            public override void OnUpdate()
            {
                _playerController._playerFallingState.Update();

            }

            public override void OnFixedUpdate()
            {
                _playerController._playerFallingState.PhysicsUpdate();
            }

            public override void OnExit()
            {
                _playerController._playerFallingState.Exit();

            }
        }


        public class LightLandingState : AbstractState
        {
            PlayerControllerStatesMachine _playerController;
            public override void OnEnter()
            {
                _playerController = GetStateMachine<PlayerControllerStatesMachine>();
                _playerController._playerLightLandingState.Enter();
            }
            public override void OnUpdate()
            {
                _playerController._playerLightLandingState.Update();

            }

            public override void OnFixedUpdate()
            {
                _playerController._playerLightLandingState.PhysicsUpdate();
            }

            public override void OnExit()
            {
                _playerController._playerLightLandingState.Exit();

            }
        }

        public class MediumStoppingState : AbstractState
        {
            PlayerControllerStatesMachine _playerController;
            public override void OnEnter()
            {
                _playerController = GetStateMachine<PlayerControllerStatesMachine>();
                _playerController._playerMediumStoppingState.Enter();
            }
            public override void OnUpdate()
            {
                _playerController._playerMediumStoppingState.Update();

            }

            public override void OnFixedUpdate()
            {
                _playerController._playerMediumStoppingState.PhysicsUpdate();
            }

            public override void OnExit()
            {
                _playerController._playerMediumStoppingState.Exit();

            }
        }



        private void OnTriggerEnter(Collider collider)
        {
            TriggerEnterEvent.Invoke(collider);
        }

        private void OnTriggerExit(Collider collider)
        {
            TriggerExitEvent.Invoke(collider);
        }




    }
}
