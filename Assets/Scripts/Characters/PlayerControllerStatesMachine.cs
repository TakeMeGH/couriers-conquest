using UnityEngine;
using CC.Characters.DataBlueprint.Layers;
using UnityEngine.Events;
using System.Collections.Generic;
using CC.Combats;
using CC.Core.Data.Dynamic;
using CC.Events;
using SA;
using CC.Ragdoll;
using CC.Interaction;
using CC.Characters.States;

namespace CC.Characters
{
    public class PlayerControllerStatesMachine : StateMachine.StateMachine
    {
        [field: Header("Input Reader")]
        [field: SerializeField] public InputReader InputReader { get; private set; }

        [field: Header("Player Movement Data")]
        [field: SerializeField] public PlayerMovementSO PlayerMovementData { get; private set; }

        [field: Header("Animation Events")]
        [field: SerializeField] public VoidEventChannelSO TriggerOnMovementStateAnimationEnterEvent { get; private set; }
        [field: SerializeField] public VoidEventChannelSO TriggerOnMovementStateAnimationExitEvent { get; private set; }
        [field: SerializeField] public VoidEventChannelSO TriggerOnMovementStateAnimationTransitionEvent { get; private set; }

        [field: Header("Events")]
        [SerializeField] VoidEventChannelSO _onPlayerDead;
        [SerializeField] IntEventChannelSO _playerWatcher;
        [SerializeField] IntEventChannelSO _onUpdateExp;

        [field: Header("Collisions")]
        [field: SerializeField] public PlayerLayerData LayerData { get; private set; }

        [field: Header("Player Stats")]
        [field: SerializeField] public PlayerStatsSO PlayerStatsSO { get; private set; }

        [field: Header("Attack Combo")]
        [field: SerializeField] public AttackSO[] Attacks { get; private set; }
        [field: SerializeField] public WeaponDamage Weapon { get; private set; }

        [field: Header("Blocking")]
        [field: SerializeField] public BlockSO Block { get; private set; }

        [field: Header("Health")]
        [field: SerializeField] public Health Health { get; private set; }

        [field: Header("Climbing")]
        [field: SerializeField] public FreeClimbMC FreeClimb { get; private set; }
        [field: SerializeField] public Transform StandUpPoint { get; private set; }
        public Vector3 OffsetStandUpPoint { get; private set; }

        [field: Header("Stamina")]
        [field: SerializeField] public StaminaController StaminaController { get; private set; }

        [field: Header("Ragdoll")]
        [field: SerializeField] private RagdollController _ragdollController;
        #region Variable
        int _playerWatcherCount = 0;
        #endregion

        #region Component
        public Animator Animator { get; private set; }
        public Rigidbody Rigidbody { get; private set; }
        public Transform MainCameraTransform { get; private set; }
        public PlayerStateData PlayerCurrentData { get; private set; }
        public PlayerResizableCapsuleCollider ResizableCapsuleCollider { get; private set; }
        public InteractionManager _interactionManager { get; private set; }

        #endregion

        #region Events
        public event UnityAction<Collider> TriggerEnterEvent = delegate { };
        public event UnityAction<Collider> TriggerExitEvent = delegate { };
        #endregion

        #region State
        public States.PlayerIdlingState PlayerIdlingState { get; private set; }
        public States.PlayerRuningState PlayerRuningState { get; private set; }
        public States.PlayerDashingState PlayerDashingState { get; private set; }
        public States.PlayerSprintingState PlayerSprintingState { get; private set; }
        public States.PlayerJumpingState PlayerJumpingState { get; private set; }
        public States.PlayerFallingState PlayerFallingState { get; private set; }
        public States.PlayerLightLandingState PlayerLightLandingState { get; private set; }
        public States.PlayerMediumStoppingState PlayerMediumStoppingState { get; private set; }
        public States.PlayerBlockingState PlayerBlockingState { get; private set; }
        public States.PlayerWalkingState PlayerWalkingState { get; private set; }
        public States.PlayerClimbState PlayerClimbState { get; private set; }
        public States.PlayerClimbUpState PlayerClimbUpState { get; private set; }
        public List<States.PlayerAttackingState> PlayerAttackingStates { get; private set; }

        #endregion
        private void Initialize()
        {
            Rigidbody = GetComponent<Rigidbody>();
            ResizableCapsuleCollider = GetComponent<PlayerResizableCapsuleCollider>();
            Animator = GetComponentInChildren<Animator>();
            StaminaController = GetComponent<StaminaController>();
            _interactionManager = GetComponent<InteractionManager>();

            PlayerCurrentData = new PlayerStateData();

            MainCameraTransform = UnityEngine.Camera.main.transform;
            PlayerCurrentData.TimeToReachTargetRotation = PlayerMovementData.TargetRotationReachTime;

            PlayerIdlingState = new States.PlayerIdlingState(this);
            PlayerRuningState = new States.PlayerRuningState(this);
            PlayerDashingState = new States.PlayerDashingState(this);
            PlayerSprintingState = new States.PlayerSprintingState(this);
            PlayerJumpingState = new States.PlayerJumpingState(this);
            PlayerFallingState = new States.PlayerFallingState(this);
            PlayerLightLandingState = new States.PlayerLightLandingState(this);
            PlayerMediumStoppingState = new States.PlayerMediumStoppingState(this);
            PlayerBlockingState = new States.PlayerBlockingState(this);
            PlayerWalkingState = new States.PlayerWalkingState(this);
            PlayerClimbState = new States.PlayerClimbState(this);
            PlayerClimbUpState = new States.PlayerClimbUpState(this);

            PlayerAttackingStates = new List<States.PlayerAttackingState>();
            for (int i = 0; i < Attacks.Length; i++)
            {
                States.PlayerAttackingState newAttackingState = new States.PlayerAttackingState(this, i);
                PlayerAttackingStates.Add(newAttackingState);
            }

            OffsetStandUpPoint = StandUpPoint.transform.position - transform.position;

            Weapon.SetStats(PlayerStatsSO);
            Health.SetStats(PlayerStatsSO);
            StaminaController.SetStats(PlayerStatsSO);

            _playerWatcher.OnEventRaised += OnEnemyWatch;
            _onUpdateExp.OnEventRaised += PlayerStatsSO.OnUpdateExp;
        }

        private void Start()
        {
            Initialize();
            SwitchState(PlayerIdlingState);
        }

        void OnEnemyWatch(int value)
        {
            _playerWatcherCount += value;
            if (_playerWatcherCount > 0)
            {
                _interactionManager.CanInteract = false;
            }
            else
            {
                _interactionManager.CanInteract = true;
            }
        }
        public void OnDead()
        {
            OnDestroy();

            Rigidbody.isKinematic = true;
            Rigidbody.useGravity = false;
            currentState = null;

            AudioManager.instance.InitializeBGM(AudioManager.instance.DeadBGM);
            _ragdollController.SetRagdoll(true, false);
            _onPlayerDead.RaiseEvent();
        }

        private void OnTriggerEnter(Collider collider)
        {
            TriggerEnterEvent.Invoke(collider);
        }

        private void OnTriggerExit(Collider collider)
        {
            TriggerExitEvent.Invoke(collider);
        }

        private void OnDestroy()
        {
            if (currentState != null) ((PlayerMovementState)currentState).OnDestroy();
        }
    }
}