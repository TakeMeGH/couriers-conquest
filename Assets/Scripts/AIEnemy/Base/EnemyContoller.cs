using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CC.Enemy
{
    public class Enemy : StateMachine.StateMachine
    {
        [field: SerializeField] public float MaxHealth { get; set; } = 100f;
        

        public float CurrentHealth { get; set; }
        public Rigidbody Rigidbody { get; set; }
        


        #region State Machine Variables
        private StateMachine.StateMachine stateMachine;
        public States.EnemyIdleState IdleState { get; private set; }
        public States.EnemyPatrolingState PatrolingState { get; private set; }
        public States.EnemyChasingState ChasingState { get; private set; }
        public States.EnemyAttackState AttackState { get; private set; }
        #endregion

        public void Initialize()
        {
            IdleState = new States.EnemyIdleState(this);
            PatrolingState = new States.EnemyPatrolingState(this);
            ChasingState = new States.EnemyChasingState(this);
            AttackState = new States.EnemyAttackState(this);
        }

        public void Start()
        {
            CurrentHealth = MaxHealth;
            Rigidbody = GetComponent<Rigidbody>();

            Initialize();
            SwitchState(IdleState);
            
        }

        public void Damage(float damageAmount)
        {
            CurrentHealth -= damageAmount;

            if(CurrentHealth < 0)
            {
                Die();
            }
        }

        public void Die()
        {
        
        }

        #region Movement Functions

        public void Chasing(Vector2 velocity)
        {
        
        }

        public void Patroling(Vector2 velocity)
        {
            
        }

        #endregion

        #region Animation Trigger

        private void OnAnimationEnterEvent(AnimationTriggerType triggerType)
        {
            
        }

        public enum AnimationTriggerType
        {
            EnemyDamage
        }

        #endregion
    }
}
