using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Combats
{

    public class Health : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 100;

        private int health;

        private bool isInvunerable;

        private void Start()
        {
            health = maxHealth;
        }

        public void SetInvunerable(bool isInvunerable)
        {
            this.isInvunerable = isInvunerable;
        }

        public void DealDamage(int damage)
        {
            if (health == 0) { return; }

            if (isInvunerable) { return; }

            health = Mathf.Max(health - damage, 0);

            Debug.Log(health);
        }
    }
}