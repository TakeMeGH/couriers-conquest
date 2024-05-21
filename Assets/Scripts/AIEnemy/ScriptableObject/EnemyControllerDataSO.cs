using UnityEngine;

namespace CC
{
    [CreateAssetMenu(fileName = "Enemy Controller Data SO", menuName = "Game/Enemy Controller Data")]
    public class EnemyControllerDataSO : ScriptableObject
    {
        [field: SerializeField] public LayerMask PlayerMask;
        [field: SerializeField] public LayerMask ObstacleMask;
        [field: SerializeField] public Vector3 EnemyRayOffset;
        [field: SerializeField] public float NavmeshRayMaxDistance = 1f;
        [field: SerializeField] public int DefaultAnggularSpeed = 360;
        [field: SerializeField] public float DefaultStopingDistance = 0.33f;

        [field: SerializeField] public float StartWaitTime = 4;
        [field: SerializeField] public float TimetoRotate = 2;
        #region Patrol Data
        [field: SerializeField, Header("patrol Data")] public float PatrolSpeed = 3f;
        [field: SerializeField] public float PatrolStopDistance = 0.33f;
        #endregion

        #region Chasing Data
        [field: SerializeField, Header("Chasing Data")] public float FastChaseSpeed = 10f;
        [field: SerializeField] public float SlowChaseSpeed = 6f;
        [field: SerializeField] public float ChaseStopingDistance = 1.7f;
        [field: SerializeField] public float TimeToReChase = 0.3f;

        #endregion
        #region Idle Attack Data
        [field: SerializeField, Header("Idle Attack Data")] public float MaxIdleTime = 3f;
        [field: SerializeField] public float MinIdleTime = 0.5f;
        #endregion

        #region Attack Data
        [field: SerializeField, Header("Attack Data")] public float LightAttackChance = 0.7f;
        [field: SerializeField] public float HeavyAttackChance = 0.3f;
        [field: SerializeField] public float AttackChaseDistancce = 1.4f;
        #endregion


        #region Taunt Data
        [field: SerializeField, Header("Taunt Data")] public int TauntCount = 1;
        #endregion

        #region Step Back Data
        [field: SerializeField, Header("Step Back Data")] public float StepBackSpeed = 1.5f;
        [field: SerializeField] public float StepBackDistance = 5f;
        [field: SerializeField] public float StepBackMaxTime = 3f;
        [field: SerializeField] public float TimeToReStepBack = 0.2f;


        #endregion

        #region View Data
        [field: SerializeField, Header("View Data")] public float ViewRadius = 10;
        [field: SerializeField] public float ViewAngle = 240;
        #endregion

    }
}