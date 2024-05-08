using System.Collections;
using System.Collections.Generic;
using CC.Enemy;
using UnityEngine;

namespace CC.Enemy.States
{
    public class EnemyChasingState : EnemyState
    {
        public EnemyChasingState(EnemyController enemy) : base(enemy)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Chasing");
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
            Debug.Log("Chasing3");
            // EnviromentView();

            // if (!enemy.m_PlayerNear)
            // {
            //     Debug.Log("Chasing2");
            //     Chasing();
            // }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            Chasing();

        }

        public override void OnAnimationEnterEvent()
        {
            base.OnAnimationEnterEvent();
        }
        private void Chasing()
        {
            Debug.Log("Chasing...");
            enemy.m_PlayerNear = false;
            enemy.playerLastPosition = Vector3.zero;

            if (!enemy.m_CaughtPlayer)
            {
                if (Vector3.Distance(enemy.transform.position, enemy.EnemyCurrentData.m_PlayerPosition) <= enemy.stopDistanceFromPlayer)
                {
                    Debug.Log("Chasing12");
                    Stop();
                    Vector3 targetDirection = enemy.EnemyCurrentData.m_PlayerPosition - enemy.transform.position;
                    targetDirection = new Vector3(targetDirection.x, 0, targetDirection.z);
                    Vector3 newDirection = Vector3.RotateTowards(enemy.transform.forward, targetDirection, 1, 0.0f);
                    enemy.transform.rotation = Quaternion.LookRotation(newDirection);
                    enemy.Animator.SetBool("isAttack", true);
                }
                else
                {
                    Debug.Log("Chasing123");
                    Move(enemy.speedRun);
                    enemy.navMeshAgent.SetDestination(enemy.EnemyCurrentData.m_PlayerPosition);
                    enemy.Animator.SetBool("isChasing", true);
                    enemy.Animator.SetBool("isAttack", false);
                }
            }
            if (enemy.navMeshAgent.remainingDistance <= enemy.navMeshAgent.stoppingDistance)
            {
                Debug.Log("Chasing23");
                if (enemy.m_WaitTime <= 0 && !enemy.m_CaughtPlayer && Vector3.Distance(enemy.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
                {
                    Debug.Log("Chasing234");
                    enemy.m_IsPatrol = true;
                    enemy.m_PlayerNear = false;
                    Move(enemy.speedWalk);
                    enemy.m_TimeToRotate = enemy.timetoRotate;
                    enemy.m_WaitTime = enemy.startWaitTime;
                    enemy.navMeshAgent.SetDestination(enemy.waypoints[enemy.m_CurrentWaypointIndex].position);
                }
                else
                {
                    Debug.Log("Chasing321");
                    if (Vector3.Distance(enemy.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                    {
                        Stop();
                        enemy.m_WaitTime -= Time.deltaTime;
                    }
                }
            }
        }
    }
}