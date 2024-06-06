using UnityEngine;
using UnityEngine.AI;
using CC.Enemy.States;
using CC.Combats;
using CC.Core.Data.Dynamic;
using CC.Ragdoll;
using System.Collections;
using CC.UI;
using CC.Events;
using CC.Inventory;
using System.Collections.Generic;
using UnityEngine.Events;

namespace CC.Enemy
{
    public class EnemyController : StateMachine.StateMachine
    {

        [Header("Data")]
        public EnemyControllerDataSO EnemyPersistenceData;
        public Transform[] PatrolWaypoints;
        public UnityAction OnPlayerDetected;
        public UnityAction OnEnemyDead;

        public bool IsDead { get; private set; }
        [Header("Drop Item")]
        [SerializeField] EnemyDropItemSO _dropItemData;
        [SerializeField] Vector3 _dropItemOffset;
        [SerializeField] Vector3 _dropItemRandomForce;
        [SerializeField] float _dropSpeed;
        [field: SerializeField, Header("Component")] public WeaponDamage WeaponDamage { get; private set; }
        [SerializeField] RagdollController _ragdollController;
        [SerializeField] Health _healthController;
        [field: SerializeField] public EnemyHealthUIController HealthBar { get; private set; }
        [Header("Copy SO")]
        [SerializeField] PlayerStatsSO _enemyStatsSO;
        [SerializeField] public VoidEventChannelSO OnEnemyAttacked;
        [Header("Event")]
        [SerializeField] IntEventChannelSO _playerWatcher;
        [SerializeField] IntEventChannelSO _onUpdateExp;
        [Header("Sink Data")]
        [SerializeField] float sinkSpeed = 15f;
        [SerializeField] float sinkDuration = 3f;

        #region Component
        public NavMeshAgent NavMeshAgent { get; private set; }
        public Animator Animator { get; private set; }
        public Rigidbody Rigidbody { get; private set; }
        public EnemyStateData EnemyCurrentData { get; private set; }
        #endregion

        #region State Machine Variables
        public EnemyPatrolingState PatrolingState { get; private set; }
        public EnemyCheckingState CheckingState { get; private set; }
        public EnemyFastTauntingState FastTauntingState { get; private set; }
        public EnemyChasingState ChasingState { get; private set; }
        public EnemyFastChasingState FastChasingState { get; private set; }

        public EnemyTauntState TauntState { get; private set; }
        public EnemyIdleAttackState IdleAttackState { get; private set; }
        public EnemyStepBackState StepBackState { get; private set; }
        public EnemyAttackState AttackState { get; private set; }
        public EnemyStunedState StunedState { get; private set; }

        #endregion
        public void Initialize()
        {
            Rigidbody = GetComponent<Rigidbody>();
            Animator = GetComponentInChildren<Animator>();
            NavMeshAgent = GetComponent<NavMeshAgent>();

            EnemyCurrentData = new EnemyStateData();

            PatrolingState = new EnemyPatrolingState(this);
            CheckingState = new EnemyCheckingState(this);
            FastTauntingState = new EnemyFastTauntingState(this);
            ChasingState = new EnemyChasingState(this);
            FastChasingState = new EnemyFastChasingState(this);
            TauntState = new EnemyTauntState(this);
            AttackState = new EnemyAttackState(this);
            StepBackState = new EnemyStepBackState(this);
            IdleAttackState = new EnemyIdleAttackState(this);
            StunedState = new EnemyStunedState(this);

            var CloneStats = Instantiate(_enemyStatsSO);
            _enemyStatsSO = CloneStats;

            var CloneAttackedEvent = Instantiate(OnEnemyAttacked);
            OnEnemyAttacked = CloneAttackedEvent;

            if (_healthController != null)
            {
                _healthController.gameObject.SetActive(true);
                _healthController.SetStats(_enemyStatsSO);
                _healthController.SetAttackEvent(OnEnemyAttacked);

            }


            if (HealthBar != null)
            {
                HealthBar.gameObject.SetActive(true);
            }

            if (WeaponDamage != null)
            {
                WeaponDamage.SetStats(_enemyStatsSO);

            }

            if (HealthBar != null)
            {
                HealthBar.SetStats(_enemyStatsSO);
            }

            if (_ragdollController != null)
            {
                _ragdollController.Initialize();
            }


            IsDead = false;
            SwitchState(PatrolingState);

        }

        void Start()
        {
            Initialize();
        }

        public void OnDead()
        {
            OnEnemyAttacked.OnEventRaised -= OnAttacked;

            Rigidbody.isKinematic = true;
            Rigidbody.useGravity = false;
            NavMeshAgent.enabled = false;
            currentState = null;

            HealthBar.gameObject.SetActive(false);
            _healthController.gameObject.SetActive(false);

            IsDead = true;

            PlayerOutOfRange();

            List<DropedItem> dropedItems = _dropItemData.GetDroppedItems();

            _ragdollController.SetRagdoll(true, false);

            _onUpdateExp.RaiseEvent(_dropItemData.Exp);

            StartCoroutine(DropItemsWithDelay(dropedItems));
            StartCoroutine(SinkAndFade());

        }

        IEnumerator SinkAndFade()
        {
            float timer = 0;
            Vector3 startPosition = transform.position;

            yield return new WaitForSeconds(2);
            _ragdollController.SetRagdoll(false, false);

            while (timer < sinkDuration)
            {
                transform.position = Vector3.Lerp(startPosition, startPosition - Vector3.up * sinkSpeed, timer / sinkDuration);
                timer += Time.deltaTime;
                yield return null;
            }

            OnEnemyDead.Invoke();
            Destroy(gameObject);
        }

        private IEnumerator DropItemsWithDelay(List<DropedItem> droppedItems)
        {
            foreach (var item in droppedItems)
            {
                DropItem(item.Item, item.Amount);
                yield return new WaitForSeconds(0.1f);
            }
        }


        void DropItem(ABaseItem item, int amount)
        {
            GameObject dropPrefab = ObjectPooling.SharedInstance.GetPooledObject(PoolObjectType.Item);
            if (dropPrefab != null)
            {
                dropPrefab.transform.position = transform.position + _dropItemOffset;
                dropPrefab.transform.rotation = transform.rotation;
                dropPrefab.SetActive(true);
            }

            Rigidbody rb = dropPrefab.GetComponent<Rigidbody>();
            if (rb != null) rb.velocity = transform.up * _dropSpeed;

            ItemPickup ip = dropPrefab.GetComponentInChildren<ItemPickup>();
            ip.isDropItem = true;
            if (ip != null)
            {
                ip.item = item;
                ip.amount = amount;
            }
        }

        public void PlayerInRange()
        {
            if (EnemyCurrentData.IsPlayerInRange == false)
            {
                EnemyCurrentData.IsPlayerInRange = true;
                OnPlayerDetected.Invoke();
                _playerWatcher.RaiseEvent(1);
            }
        }

        public void PlayerOutOfRange()
        {
            if (EnemyCurrentData.IsPlayerInRange == true)
            {
                EnemyCurrentData.IsPlayerInRange = false;
                _playerWatcher.RaiseEvent(-1);
            }
        }

        public bool IsPlayerInRange()
        {
            return EnemyCurrentData.IsPlayerInRange;
        }


        void OnAttacked()
        {
            ((EnemyControllerState)currentState).OnAttacked();
            PlayerInRange();
        }
        public void OnAnimationEnterEvent()
        {
            currentState.OnAnimationEnterEvent();
        }

        public void OnAnimationExitEvent()
        {
            currentState.OnAnimationExitEvent();
        }

        public void OnAnimationTransitionEvent()
        {
            currentState.OnAnimationTransitionEvent();
        }

    }
}
