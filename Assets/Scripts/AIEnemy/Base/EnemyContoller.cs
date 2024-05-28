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

namespace CC.Enemy
{
    public class EnemyController : StateMachine.StateMachine
    {

        [Header("Data")]
        public EnemyControllerDataSO EnemyPersistenceData;
        public Transform[] PatrolWaypoints;
        [Header("Drop Item")]
        [SerializeField] EnemyDropItemSO _dropItemData;
        [SerializeField] Vector3 _dropItemOffset;
        [SerializeField] Vector3 _dropItemRandomForce;
        [SerializeField] float _dropSpeed;
        [field: SerializeField, Header("Component")] public WeaponDamage WeaponDamage { get; private set; }
        [SerializeField] RagdollController _ragdollController;
        [SerializeField] Health _healthController;
        [SerializeField] float _multiplier;
        [SerializeField] EnemyHealthUIController _healthBar;
        [Header("Copy SO")]
        [SerializeField] PlayerStatsSO _enemyStatsSO;
        [SerializeField] VoidEventChannelSO _onEnemyAttacked;
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

            var CloneAttackedEvent = Instantiate(_onEnemyAttacked);
            _onEnemyAttacked = CloneAttackedEvent;
            _onEnemyAttacked.OnEventRaised += OnAttacked;

            _healthController.SetStats(_enemyStatsSO);
            _healthController.SetAttackEvent(_onEnemyAttacked);

            WeaponDamage.SetStats(_enemyStatsSO);
            _healthBar.SetStats(_enemyStatsSO);


        }

        void Start()
        {
            Initialize();
            SwitchState(PatrolingState);
        }

        public void OnDead()
        {
            _onEnemyAttacked.OnEventRaised -= OnAttacked;

            Rigidbody.isKinematic = true;
            Rigidbody.useGravity = false;
            NavMeshAgent.enabled = false;
            currentState = null;
            Destroy(_healthBar.gameObject);

            List<DropedItem> dropedItems = _dropItemData.GetDroppedItems();

            _ragdollController.SetRagdoll(true, false);

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

        void OnAttacked()
        {
            ((EnemyControllerState)currentState).OnAttacked();
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
