using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using CC.Enemy.States;


namespace CC.Enemy
{
    public class EnemyController : StateMachine.StateMachine
    {
        [field: SerializeField] public float MaxHealth { get; set; } = 100f;
        [SerializeField] public LayerMask playerMask;
        [SerializeField] public NavMeshAgent navMeshAgent;

        [SerializeField] public float timetoRotate = 2;
        [SerializeField] public float speedWalk = 6;
        [SerializeField] public float startWaitTime = 4;
        [SerializeField] public float speedRun = 9;
        [SerializeField] public float viewRadius = 15;
        [SerializeField] public float viewAngle = 90;
        [SerializeField] public float meshResolution = 1f;
        [SerializeField] public int edgeIterations = 4;
        [SerializeField] public float edgeDistance = 0.5f;
        [SerializeField] public float stoppingDistance = 0.01f;
        [SerializeField] public float stopDistanceFromPlayer = 2f;
        [SerializeField] public float rotationSpeed = 5f;
        [SerializeField] public float attackRange = 1.5f;
        [SerializeField] public float maxWaitTime;

        public Vector3 playerLastPosition = Vector3.zero;
        public Vector3 m_PlayerPosition;

        public float m_WaitTime;
        public float m_TimeToRotate;
        public bool m_PlayerInRange;
        public bool m_PlayerNear;
        public bool m_IsPatrol;
        public bool m_CaughtPlayer;
        public int m_CurrentWaypointIndex = 0;

        public float CurrentHealth { get;  private set; }

        #region Component
        public Animator Animator { get; private set; }
        public Rigidbody Rigidbody { get; private set; }
        public EnemyStateData EnemyCurrentData { get; private set; }
        public EnemyState enemyState { get; private set; }
        #endregion

        public Transform[] waypoints;

        #region State Machine Variables
        private StateMachine.StateMachine stateMachine;
        public States.EnemyIdleState IdleState { get; private set; }
        public States.EnemyPatrolingState PatrolingState { get; private set; }
        public States.EnemyChasingState ChasingState { get; private set; }
        public States.EnemyAttackState AttackState { get; private set; }
        #endregion

        public void Initialize()
        {
            CurrentHealth = MaxHealth;
            Rigidbody = GetComponent<Rigidbody>();
            Animator = GetComponentInChildren<Animator>();

            EnemyCurrentData = new EnemyStateData();
            EnemyCurrentData.navMeshAgent = GetComponent<NavMeshAgent>();

            IdleState = new States.EnemyIdleState(this);

            PatrolingState = new States.EnemyPatrolingState(this);
            ChasingState = new States.EnemyChasingState(this);
            AttackState = new States.EnemyAttackState(this);

            EnemyCurrentData.m_PlayerPosition = Vector3.zero;
            EnemyCurrentData.m_IsPatrol = true;
            EnemyCurrentData.m_CaughtPlayer = false;
            EnemyCurrentData.m_PlayerInRange = false;
            EnemyCurrentData.m_WaitTime = EnemyCurrentData.startWaitTime;
            EnemyCurrentData.m_TimeToRotate = EnemyCurrentData.timetoRotate;

            EnemyCurrentData.m_CurrentWaypointIndex = 0;
            EnemyCurrentData.navMeshAgent = GetComponent<NavMeshAgent>();
            EnemyCurrentData.navMeshAgent.stoppingDistance = EnemyCurrentData.stoppingDistance;

            EnemyCurrentData.navMeshAgent.isStopped = false;
            EnemyCurrentData.navMeshAgent.speed = EnemyCurrentData.speedWalk;
            EnemyCurrentData.navMeshAgent.SetDestination(waypoints[EnemyCurrentData.m_CurrentWaypointIndex].position);

            EnemyCurrentData.m_WaitTime = Random.Range(0f, EnemyCurrentData.maxWaitTime);
            waypoints = GameObject.FindGameObjectsWithTag("Waypoint").Select(obj => obj.transform).ToArray();

            if (waypoints.Length > 0)
            {
                EnemyCurrentData.navMeshAgent.SetDestination(waypoints[EnemyCurrentData.m_CurrentWaypointIndex].position);
            }
            else
            {
                Debug.LogError("No waypoints found!");
            }

            EnemyCurrentData.m_WaitTime = Random.Range(0f, EnemyCurrentData.maxWaitTime);
        }

        void Start()
        {
            Initialize();
            //enemyState = new EnemyChasingState(this);
            SwitchState(IdleState);
        }

        void Update()
        {
            //enemyState.EnviromentView();
        }

        public void Damage(float damageAmount)
        {
            CurrentHealth -= damageAmount;

            if(CurrentHealth < 0)
            {
                Die();
            }
        }

        public void Die()
        {
        
        }
    }
}
