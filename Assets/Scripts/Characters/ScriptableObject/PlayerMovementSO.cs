using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Characters
{
    [CreateAssetMenu(fileName = "PlayerMovement", menuName = "Game/Player Movement")]
    public class PlayerMovementSO : ScriptableObject
    {
        [field: SerializeField][field: Range(0f, 25f)] public float BaseSpeed { get; private set; } = 5f;
        [field: SerializeField] public Vector3 TargetRotationReachTime { get; private set; }


        #region Grounded Data
        [field: SerializeField][field: Range(0f, 25f)] public float GroundedBaseSpeed { get; private set; } = 5f;
        [field: SerializeField][field: Range(0f, 5f)] public float GroundToFallRayDistance { get; private set; } = 1f;
        [field: SerializeField] public AnimationCurve SlopeSpeedAngles { get; private set; }

        #region RunData
        [field: SerializeField][field: Range(1f, 2f)] public float RunSpeedModifier { get; private set; } = 1f;
        #endregion

        #region RunData
        [field: SerializeField][field: Range(1f, 3f)] public float SprintSpeedModifier { get; private set; } = 1.7f;
        #endregion

        #region DashData
        [field: SerializeField][field: Range(1f, 3f)] public float DashSpeedModifier { get; private set; } = 2f;
        [field: SerializeField] public Vector3 DashTargetRotationReachTime { get; set; }
        #endregion

        #region SprintData
        [field: SerializeField][field: Range(0f, 5f)] public float SprintToRunTime { get; private set; } = 1f;
        #endregion


        // [field: SerializeField] public List<PlayerCameraRecenteringData> SidewaysCameraRecenteringData { get; private set; }
        // [field: SerializeField] public List<PlayerCameraRecenteringData> BackwardsCameraRecenteringData { get; private set; }
        // [field: SerializeField] public PlayerRotationData BaseRotationData { get; private set; }
        // [field: SerializeField] public PlayerIdleData IdleData { get; private set; }
        // [field: SerializeField] public PlayerDashData DashData { get; private set; }
        // [field: SerializeField] public PlayerWalkData WalkData { get; private set; }
        // [field: SerializeField] public PlayerRunData RunData { get; private set; }
        // [field: SerializeField] public PlayerSprintData SprintData { get; private set; }
        // [field: SerializeField] public PlayerStopData StopData { get; private set; }
        // [field: SerializeField] public PlayerRollData RollData { get; private set; }

        #endregion

    }

}

