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
        [field: SerializeField, Header("patrol Data")] public float PatrolStopDistance = 0.33f;
        #endregion

        #region Chasing Data
        [field: SerializeField, Header("Chasing Data")] public float FastChaseSpeed = 3f;
        [field: SerializeField] public float SlowChaseSpeed = 2f;
        [field: SerializeField] public float ChaseStopingDistancce = 1.8f;
        [field: SerializeField] public float TooCloseDistance = 1f;
        [field: SerializeField] public float TimeToReChase = 0.3f;

        #endregion

        #region Taunt Data
        [field: SerializeField, Header("Taunt Data")] public int TauntCount = 3;
        #endregion

        #region Step Back Data
        [field: SerializeField, Header("Step Back Data")] public float StepBackSpeed = 1.5f;
        [field: SerializeField] public float StepBackDistance = 2f;

        #endregion

        [field: SerializeField] public float SpeedWalk = 6;
        [field: SerializeField] public float SpeedRun = 9;
        [field: SerializeField] public float ViewRadius = 2;
        [field: SerializeField] public float ViewAngle = 90;
        [field: SerializeField] public float MeshResolution = 1f;
        [field: SerializeField] public int EdgeIterations = 4;
        [field: SerializeField] public float EdgeDistance = 0.5f;
        [field: SerializeField] public float StoppingDistance = 0.01f;
        [field: SerializeField] public float StopDistanceFromPlayer = 2f;
        [field: SerializeField] public float RotationSpeed = 5f;
        [field: SerializeField] public float AttackRange = 1.5f;
        [field: SerializeField] public float MaxWaitTime;
    }
}