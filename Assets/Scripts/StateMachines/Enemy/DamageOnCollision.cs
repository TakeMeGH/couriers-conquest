using UnityEngine;

namespace CC.Combats
{
    public class DamageOnCollision : MonoBehaviour
    {
        [SerializeField] private float damageAmount = 0f;
        [SerializeField] private float damageRate = 0f;  // Damage per second
        private float nextDamageTime = 0f;

        private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time >= nextDamageTime)
        {
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.DealDamage(Mathf.RoundToInt(damageAmount));
                nextDamageTime = Time.time + 1f/damageRate;
                Debug.Log("Damage Amount: " + damageAmount);
                Debug.Log("Player Health: " + playerHealth.GetCurrentHealth());
            }
        }
    }


    }
}



/*private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player") && Time.time >= nextDamageTime)
            {
                Health playerHealth = collision.gameObject.GetComponent<Health>();
                if (playerHealth != null)
                {
                    playerHealth.DealDamage(Mathf.RoundToInt(damageAmount));
                    nextDamageTime = Time.time + 1f/damageRate;
                    Debug.Log("Damage Amount: " + damageAmount);
                    Debug.Log("Player Health: " + playerHealth.GetCurrentHealth());
                }
            }
        }
        */
/*using UnityEngine;

namespace CC.Combats
{
    public class DamageOnCollision : MonoBehaviour
    {
        private float damageAmount = 2f;

        private void OnTriggerStay(Collider other)
        {
            Debug.Log("Trigger Stay Detected");

            if (other.CompareTag("Player"))
            {
                Debug.Log("Player Tag Detected");

                Health playerHealth = other.GetComponent<Health>();
                if (playerHealth != null)
                {
                    float calculatedDamage = damageAmount * Time.deltaTime;
                    int roundedDamage = Mathf.RoundToInt(calculatedDamage);
                    Debug.Log("Damage Amount: " + damageAmount);
                    Debug.Log("Time.deltaTime: " + Time.deltaTime);
                    Debug.Log("Calculated Damage: " + roundedDamage);

                    playerHealth.DealDamage(roundedDamage);
                    Debug.Log("Player Health: " + playerHealth.GetCurrentHealth());
                }
            }
        }
    }
}
*/