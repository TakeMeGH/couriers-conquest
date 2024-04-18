/*using UnityEngine;

namespace CC.Combats
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 100;

        private int health;

        private bool isInvunerable;

        private void Start()
        {
            health = maxHealth;
        }

        public int GetCurrentHealth()
        {
            return health;
        }

        public void SetInvunerable(bool isInvunerable)
        {
            this.isInvunerable = isInvunerable;
        }

        public void DamageReceive(int damage)
        {
            if (health == 0) { return; }

            if (isInvunerable) { return; }

            health = Mathf.Max(health - damage, 0);
            Debug.Log("Player Health after Damage: " + health);
        }
    }
}
*/