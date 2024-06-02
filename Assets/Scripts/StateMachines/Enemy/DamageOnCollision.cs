using UnityEngine;
using CC.Characters;
using CC.Characters.States;
using UnityEngine.VFX;

namespace CC.Combats
{
    public class DamageOnCollision : MonoBehaviour
    {
        [SerializeField] private float damageAmount = 0f;
        [SerializeField] private float damageRate = 0f;  // Damage per second
        [SerializeField] private GameObject vfxHitBanditPrefab; // Assign in the inspector
        [SerializeField] private GameObject vfxHitGoblinPrefab; // Assign in the inspector

        private float nextDamageTime = 0f;

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && Time.time >= nextDamageTime)
            {
                Health playerHealth = other.gameObject.GetComponent<Health>();
                PlayerControllerStatesMachine playerController = other.gameObject.GetComponent<PlayerControllerStatesMachine>();
                StaminaController staminaController = other.gameObject.GetComponent<StaminaController>();
                if (playerHealth != null && playerController != null && staminaController != null)
                {
                    playerHealth.DealDamage(Mathf.RoundToInt(damageAmount));
                    if (playerController.GetCurrentState() is PlayerBlockingState)
                    {
                        float blockStaminaCost = playerController.PlayerMovementData.BlockStaminaCost;
                        staminaController.DecreaseStaminaByAmount(blockStaminaCost);
                    }
                    nextDamageTime = Time.time + 1f / damageRate;
                    Debug.Log("Damage Amount: " + damageAmount);
                    Debug.Log("Player Health: " + playerHealth.GetCurrentHealth());

                    // Trigger VFX based on the enemy tag
                    // TriggerVFX(collision.gameObject.tag);
                }
            }

        }

        private void TriggerVFX(string enemyTag)
        {
            Vector3 vfxPosition = transform.position + new Vector3(0, 1, 0); // Adjust the position as needed
            GameObject vfxHitInstance = null;

            if (enemyTag == "Bandit")
            {
                vfxHitInstance = Instantiate(vfxHitBanditPrefab, vfxPosition, Quaternion.identity);
            }
            else if (enemyTag == "Goblin")
            {
                vfxHitInstance = Instantiate(vfxHitGoblinPrefab, vfxPosition, Quaternion.identity);
            }

            if (vfxHitInstance != null)
            {
                VisualEffect vfxHit = vfxHitInstance.GetComponent<VisualEffect>();
                 vfxHit.SendEvent("OnPlay");
            }
        }
    }
}