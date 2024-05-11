using UnityEngine;
using UnityEngine.AI;
using CC.Enemy.States;


namespace CC.Enemy
{
    public class EnemyController : StateMachine.StateMachine
    {

        public EnemyControllerDataSO EnemyPersistenceData;
        public Transform[] PatrolWaypoints;
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

            // NavMeshAgent.stoppingDistance = EnemyPersistenceData.StoppingDistance;

            // NavMeshAgent.isStopped = false;
            // NavMeshAgent.speed = EnemyPersistenceData.SpeedWalk;
            // NavMeshAgent.SetDestination(Waypoints[EnemyCurrentData.CurrentWaypointIndex].position);

            // EnemyCurrentData.CurrentWaitTime = Random.Range(0f, EnemyPersistenceData.MaxWaitTime);
            // Waypoints = GameObject.FindGameObjectsWithTag("Waypoint").Select(obj => obj.transform).ToArray();

            // if (Waypoints.Length > 0)
            // {
            //     NavMeshAgent.SetDestination(Waypoints[EnemyCurrentData.CurrentWaypointIndex].position);
            // }
            // else
            // {
            //     Debug.LogError("No waypoints found!");
            // }

            // EnemyCurrentData.CurrentWaitTime = Random.Range(0f, EnemyPersistenceData.MaxWaitTime);
        }

        void Start()
        {
            Initialize();
            SwitchState(PatrolingState);
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
