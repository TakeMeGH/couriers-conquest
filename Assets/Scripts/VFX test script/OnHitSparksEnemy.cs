/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class OnHitSparksEnemy : MonoBehaviour
{
    // Assign your VFX prefabs in the inspector
    public GameObject vfxHitBanditPrefab;
    public GameObject vfxHitGoblinPrefab;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Cast a ray from the enemy to the player
            RaycastHit hit;
            if (Physics.Raycast(transform.position, (other.transform.position - transform.position).normalized, out hit))
            {
                // If the ray hits the player, get the point of contact
                Vector3 vfxPosition = hit.point;
                Vector3 offset = hit.normal * 0.1f; // Adjust the multiplier as needed

                // Check the tag of the enemy and instantiate the corresponding VFX
                GameObject vfxHitInstance = null;
                if (gameObject.CompareTag("Bandit"))
                {
                    vfxHitInstance = Instantiate(vfxHitBanditPrefab, vfxPosition + offset, Quaternion.identity);
                }
                else if (gameObject.CompareTag("Goblin"))
                {
                    vfxHitInstance = Instantiate(vfxHitGoblinPrefab, vfxPosition + offset, Quaternion.identity);
                }

                if (vfxHitInstance != null)
                {
                    VisualEffect vfxHit = vfxHitInstance.GetComponent<VisualEffect>();
                    vfxHit.SendEvent("OnPlay");
                }
            }
        }
    }
}
*/