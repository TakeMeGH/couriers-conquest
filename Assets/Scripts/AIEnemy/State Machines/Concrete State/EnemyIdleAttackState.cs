using System.Collections;
using System.Collections.Generic;
using CC.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace CC.Enemy.States
{
    public class EnemyIdleAttackState : EnemyControllerState
    {

        float idleTime = 0;
        float currentIdleTime = 0;
        StateOption nextState;
        public EnemyIdleAttackState(EnemyController enemy) : base(enemy)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation("isIdleAttack");


            GetNextState();

            _enemyController.NavMeshAgent.speed = 0;
            _enemyController.NavMeshAgent.velocity = Vector3.zero;
            _enemyController.NavMeshAgent.isStopped = true;

            currentIdleTime = 0;

            idleTime = Random.Range(_enemyController.EnemyPersistenceData.MinIdleTime, _enemyController.EnemyPersistenceData.MaxIdleTime);
        }


        public override void Update()
        {
            base.Update();

            currentIdleTime += Time.deltaTime;
            if (currentIdleTime >= idleTime)
            {
                switch (nextState)
                {
                    case StateOption.Taunt:
                        _enemyController.SwitchState(_enemyController.FastTauntingState);
                        break;
                    case StateOption.LightAttack:
                        _enemyController.EnemyCurrentData.IsHeavyAttack = false;
                        _enemyController.SwitchState(_enemyController.FastChasingState);
                        break;
                    case StateOption.HeavyAttack:
                        _enemyController.EnemyCurrentData.IsHeavyAttack = true;
                        _enemyController.SwitchState(_enemyController.FastChasingState);
                        break;
                }
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            float distance = Vector3.Distance(_enemyController.transform.position,
_enemyController.EnemyCurrentData.PlayerTransform.transform.position);

            if (distance > _enemyController.EnemyPersistenceData.IdleAttackLimitDistance)
            {
                _enemyController.SwitchState(_enemyController.ChasingState);
            }

        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation("isIdleAttack");

        }

        void GetNextState()
        {
            float totalChance = _enemyController.EnemyPersistenceData.TauntChance + _enemyController.EnemyPersistenceData.LightAttackChance + _enemyController.EnemyPersistenceData.HeavyAttackChance;
            float randomValue = Random.Range(0, totalChance);

            if (randomValue < _enemyController.EnemyPersistenceData.TauntChance)
            {
                nextState = StateOption.Taunt;
            }
            else if (randomValue < _enemyController.EnemyPersistenceData.TauntChance
                + _enemyController.EnemyPersistenceData.LightAttackChance)
            {
                nextState = StateOption.LightAttack;
            }
            else
            {
                nextState = StateOption.HeavyAttack;
            }

        }

    }

    public enum StateOption
    {
        Taunt,
        HeavyAttack,
        LightAttack
    }
}