using System.Collections;
using System.Collections.Generic;
using CC.Enemy;
using CC.StateMachine;
using UnityEngine;
using UnityEngine.AI;

namespace CC.Enemy.States
{
    public class EnemyState : IState
    {
        protected EnemyController enemy;

        public EnemyState(EnemyController enemy)
        {
            this.enemy = enemy;
        }

        public virtual void Enter()
        {

        }

        public virtual void Exit()
        {

        }

        public virtual void Update()
        {
            EnviromentView();
            if (enemy.Animator != null)
            {
                enemy.Animator.SetFloat("Speed", enemy.EnemyCurrentData.navMeshAgent.velocity.magnitude);
            }
        }

        public virtual void PhysicsUpdate()
        {
        }

        public virtual void OnAnimationEnterEvent()
        {
        }

        public virtual void OnAnimationExitEvent()
        {
        }

        public virtual void OnAnimationTransitionEvent()
        {
        }

        public void Move(float speed)
        {
            enemy.EnemyCurrentData.navMeshAgent.isStopped = false;
            enemy.EnemyCurrentData.navMeshAgent.speed = speed;
        }

        public void Stop()
        {
            enemy.EnemyCurrentData.navMeshAgent.isStopped = true;
            enemy.EnemyCurrentData.navMeshAgent.speed = 0;
        }
        public void NextPoint(bool useRandom = true)
        {
            if (useRandom)
            {
                enemy.EnemyCurrentData.m_CurrentWaypointIndex = Random.Range(0, enemy.waypoints.Length);
            }

            enemy.EnemyCurrentData.navMeshAgent.SetDestination(enemy.waypoints[enemy.EnemyCurrentData.m_CurrentWaypointIndex].position);

        }

        void CaughtPlayer()
        {
            enemy.EnemyCurrentData.m_CaughtPlayer = true;
        }

        public void LookingPlayer(Vector3 player)
        {
            enemy.EnemyCurrentData.navMeshAgent.SetDestination(player);
            if (Vector3.Distance(enemy.transform.position, player) <= 0.3)
            {
                if (enemy.EnemyCurrentData.m_WaitTime <= 0)
                {
                    enemy.EnemyCurrentData.m_PlayerNear = false;
                    Move(enemy.EnemyCurrentData.speedWalk);
                    enemy.EnemyCurrentData.navMeshAgent.SetDestination(enemy.waypoints[enemy.EnemyCurrentData.m_CurrentWaypointIndex].position);
                    enemy.EnemyCurrentData.m_WaitTime = enemy.EnemyCurrentData.startWaitTime;
                    enemy.EnemyCurrentData.m_TimeToRotate = enemy.EnemyCurrentData.timetoRotate;
                }
                else
                {
                    Stop();
                    enemy.EnemyCurrentData.m_WaitTime -= Time.deltaTime;
                }
            }

            Vector3 direction = (player - enemy.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, lookRotation, Time.deltaTime * enemy.EnemyCurrentData.rotationSpeed);
        }
        public void EnviromentView()
        {
            Collider[] playerInRange = Physics.OverlapSphere(enemy.transform.position, enemy.EnemyCurrentData.viewRadius, enemy.playerMask);
            Debug.Log(playerInRange.Length + " " + enemy.playerMask.value + " " + enemy.transform.position + " " + enemy.EnemyCurrentData.viewRadius + " DEBUG");
            for (int i = 0; i < playerInRange.Length; i++)
            {
                Transform player = playerInRange[i].transform;
                Debug.Log(playerInRange[i].transform.gameObject.name + " NAME");
                Vector3 dirToPlayer = (player.position - enemy.transform.position).normalized;
                if (Vector3.Angle(enemy.transform.forward, dirToPlayer) < enemy.EnemyCurrentData.viewAngle / 2)
                {
                    float dstToPlayer = Vector3.Distance(enemy.transform.position, player.position);
                    if (!Physics.Raycast(enemy.transform.position, dirToPlayer, dstToPlayer, enemy.EnemyCurrentData.obstacleMask))
                    {
                        enemy.EnemyCurrentData.m_PlayerInRange = true;
                        enemy.EnemyCurrentData.m_IsPatrol = false;
                        enemy.EnemyCurrentData.m_PlayerPosition = player.position;
                    }
                    else
                    {
                        enemy.EnemyCurrentData.m_PlayerInRange = false;
                    }
                }
                if (Vector3.Distance(enemy.transform.position, player.position) > enemy.EnemyCurrentData.viewRadius)
                {
                    enemy.EnemyCurrentData.m_PlayerInRange = false;
                }
                if (enemy.EnemyCurrentData.m_PlayerInRange)
                {
                    enemy.EnemyCurrentData.m_PlayerPosition = player.transform.position;
                }
            }
        }
    }
}
