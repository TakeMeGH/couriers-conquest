using UnityEngine;

namespace CC.Combats
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private float damageReduction = 0.2f; // 20% damage reduction

        private int health;
        private bool isInvunerable;

        private void Start()
        {
            health = maxHealth;
            Debug.Log("Initial Player Health: " + health);
        }

        public int GetCurrentHealth()
        {
            return health;
        }

        public void SetInvunerable(bool isInvunerable)
        {
            this.isInvunerable = isInvunerable;
        }

        public void DealDamage(int damage)
        {  
            Debug.Log("Received Damage: " + damage);

            if (health == 0) { return; }

            if (isInvunerable) { return; }

            int calculatedDamage = Mathf.Min(Mathf.RoundToInt(damage * (1 - damageReduction)), health);
            health = Mathf.Max(health - calculatedDamage, 0);
            Debug.Log("Calculated Damage after Reduction: " + calculatedDamage);
            Debug.Log("Player Health after Damage: " + health);
        }
    }
}
