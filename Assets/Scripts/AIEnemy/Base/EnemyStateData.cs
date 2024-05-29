using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace CC.Enemy
{
    public class EnemyStateData
    {

        public int CurrentWaypointIndex = -1;
        public Transform PlayerTransform;
        public bool IsHeavyAttack;
        public bool IsPlayerInRange = false;
        public bool IsPlayerNear;
        public int AttackedCount = 0;
        public float CurrentRotationTime = 100f;
        public Quaternion TargetRotation;
        public Quaternion InitialRotation;


    }
}
