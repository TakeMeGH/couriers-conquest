using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace CC.Enemy
{
    public class EnemyStateData
    {
        public LayerMask playerMask;
        public LayerMask obstacleMask;
        public NavMeshAgent navMeshAgent;
        public float startWaitTime = 4;
        public float timetoRotate = 2;
        public float speedWalk = 6;
        public float speedRun = 9;
        public float viewRadius = 2;
        public float viewAngle = 90;
        public float meshResolution = 1f;
        public int edgeIterations = 4;
        public float edgeDistance = 0.5f;
        public float stoppingDistance = 0.01f;
        public float stopDistanceFromPlayer = 2f;
        public float rotationSpeed = 5f;
        public float attackRange = 1.5f;
        public float maxWaitTime;

        public int m_CurrentWaypointIndex = 0;

        public Vector3 playerLastPosition = Vector3.zero;
        public Vector3 m_PlayerPosition;

        public float m_WaitTime;
        public float m_TimeToRotate;
        public bool m_PlayerInRange;
        public bool m_PlayerNear;
        public bool m_IsPatrol;
        public bool m_CaughtPlayer;

    }
}
