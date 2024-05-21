using UnityEngine;
using UnityEngine.AI;
using CC.Enemy.States;
using CC.Combats;
using CC.Core.Data.Dynamic;
using CC.Ragdoll;
using System.Collections;
using CC.UI;

namespace CC.Enemy
{
    public class EnemyController : StateMachine.StateMachine
    {

        public EnemyControllerDataSO EnemyPersistenceData;
        public Transform[] PatrolWaypoints;
        [field: SerializeField] public WeaponDamage WeaponDamage { get; private set; }
        [SerializeField] RagdollController _ragdollController;

        [SerializeField] PlayerStatsSO _enemyStatsSO;
        [SerializeField] Health _healthController;
        [SerializeField] float _multiplier;
        [SerializeField] EnemyHealthUIController _healthBar;

        #region Component
        public NavMeshAgent NavMeshAgent { get; private set; }
        public Animator Animator { get; private set; }
        public Rigidbody Rigidbody { get; private set; }
        public EnemyStateData EnemyCurrentData { get; private set; }
        #endregion

        #region State Machine Variables
        public EnemyPatrolingState PatrolingState { get; private set; }
        public EnemyCheckingState CheckingState { get; private set; }
        public EnemyChasingState ChasingState { get; private set; }
        public EnemyTauntState TauntState { get; private set; }
        public EnemyIdleState IdleState { get; private set; }
        public EnemyStepBackState StepBackState { get; private set; }
        public EnemyAttackState AttackState { get; private set; }
        #endregion

        public void Initialize()
        {
            Rigidbody = GetComponent<Rigidbody>();
            Animator = GetComponentInChildren<Animator>();
            NavMeshAgent = GetComponent<NavMeshAgent>();

            EnemyCurrentData = new EnemyStateData();

            PatrolingState = new EnemyPatrolingState(this);
            CheckingState = new EnemyCheckingState(this);
            ChasingState = new EnemyChasingState(this);
            TauntState = new EnemyTauntState(this);
            AttackState = new EnemyAttackState(this);
            StepBackState = new EnemyStepBackState(this);

            IdleState = new EnemyIdleState(this);

            var CloneStats = Instantiate(_enemyStatsSO);
            _enemyStatsSO = CloneStats;

            _healthController.SetStats(_enemyStatsSO);
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
            Rigidbody.isKinematic = true;
            Rigidbody.useGravity = false;
            NavMeshAgent.enabled = false;
            currentState = null;
            Destroy(_healthBar.gameObject);

            _ragdollController.SetRagdoll(true, false);
            StartCoroutine(SinkAndFade());

        }

        public float sinkSpeed = 15f;
        public float sinkDuration = 3f;
        public float fadeDuration = 1f;
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
