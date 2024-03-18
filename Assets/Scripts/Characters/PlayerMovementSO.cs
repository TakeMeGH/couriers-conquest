using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QC.Characters
{
    [CreateAssetMenu(fileName = "PlayerMovement", menuName = "Game/Player Movement")]
    public class PlayerMovementSO : ScriptableObject
    {
        [field: SerializeField] [field: Range(0f, 25f)] public float BaseSpeed { get; private set; } = 5f;
        [field: SerializeField] public Vector3 TargetRotationReachTime { get; private set; }


        #region Grounded Data
        #endregion

    }

}

