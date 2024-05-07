using System.Collections;
using System.Collections.Generic;
using CC.Enemy;
using UnityEngine;

namespace CC.Enemy.States
{
    public class EnemyPatrolingState : EnemyState
    {
        public EnemyPatrolingState(EnemyController enemy) : base(enemy)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
            EnviromentView();

            if (enemy.EnemyCurrentData.m_PlayerInRange)
            {
                enemy.SwitchState(enemy.ChasingState);
                Debug.Log("Chasing");
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void OnAnimationEnterEvent()
        {
            base.OnAnimationEnterEvent();
        }

        private void Patroling()
        {
                if (enemy.EnemyCurrentData.navMeshAgent.remainingDistance <= enemy.EnemyCurrentData.navMeshAgent.stoppingDistance)
                {
                    if (enemy.EnemyCurrentData.m_WaitTime <= 0)
                    {
                        NextPoint();
                        Move(enemy.EnemyCurrentData.speedWalk);
                        enemy.EnemyCurrentData.m_WaitTime = Random.Range(0f, enemy.EnemyCurrentData.maxWaitTime);
                    }
                    else
                    {
                        Stop();
                        enemy.EnemyCurrentData.m_WaitTime -= Time.deltaTime;
                    }
                }
        }
    }
}
