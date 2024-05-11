using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace CC.Enemy
{
    public class EnemyStateData
    {

        public int CurrentWaypointIndex;
        public Transform PlayerTransform;
        public int IsHeavyAttack;
        public float CurrentWaitTime;
        public float CurrentTimeToRotate;
        public bool IsPlayerInRange;
        public bool IsPlayerNear;
        public bool IsPatrol;
        public bool IsCaughtPlayer;

    }
}
