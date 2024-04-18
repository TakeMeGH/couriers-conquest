using System;
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
        [field: SerializeField, Header("Grounded Data")][field: Range(0f, 25f)] public float GroundedBaseSpeed { get; private set; } = 5f;
        [field: SerializeField][field: Range(0f, 5f)] public float GroundToFallRayDistance { get; private set; } = 1f;
        [field: SerializeField] public AnimationCurve SlopeSpeedAngles { get; private set; }
        [field: SerializeField] public Vector3 GroundedTargetRotationReachTime { get; private set; }


        #region WalkData
        [field: SerializeField][field: Range(0f, 1f)] public float WalkSpeedModifier { get; private set; } = 0.225f;

        #endregion
        #region RunData
        [field: SerializeField, Header("Run Data")][field: Range(1f, 2f)] public float RunSpeedModifier { get; private set; } = 1f;
        #endregion

        #region SprintData
        [field: SerializeField, Header("Sprint Data")][field: Range(1f, 3f)] public float SprintSpeedModifier { get; private set; } = 1.7f;
        [field: SerializeField][field: Range(0f, 5f)] public float SprintToRunTime { get; private set; } = 1f;
        #endregion

        #region StopData
        [field: SerializeField, Header("Stop Data")][field: Range(0f, 15f)] public float MediumDecelerationForce { get; private set; } = 6.5f;
        #endregion

        #region DashData
        [field: SerializeField, Header("Dash Data")][field: Range(1f, 3f)] public float DashSpeedModifier { get; private set; } = 2f;
        [field: SerializeField] public Vector3 DashTargetRotationReachTime { get; set; }
        #endregion

        #region Airborne Data

        #region JumpingData
        [field: SerializeField, Header("Jump Data")][field: Range(0f, 5f)] public float JumpToGroundRayDistance { get; private set; } = 2f;
        [field: SerializeField] public AnimationCurve JumpForceModifierOnSlopeUpwards { get; private set; }
        [field: SerializeField] public AnimationCurve JumpForceModifierOnSlopeDownwards { get; private set; }

        [field: SerializeField] public Vector3 JumpTargetRotationReachTime { get; set; }
        [field: SerializeField] public Vector3 StationaryForce { get; private set; }
        [field: SerializeField] public Vector3 MediumForce { get; private set; }
        [field: SerializeField][field: Range(0f, 10f)] public float DecelerationForce { get; private set; } = 1.5f;

        #endregion

        #region FallingData
        [field: Tooltip("Having higher numbers might not read collisions with shallow colliders correctly.")]
        [field: SerializeField, Header("Fall Data")][field: Range(0f, 10f)] public float FallSpeedLimit { get; private set; } = 10f;
        #endregion

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

