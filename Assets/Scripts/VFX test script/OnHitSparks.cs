using UnityEngine;
using UnityEngine.VFX;

public class OnHitSparks : MonoBehaviour
{
    public GameObject vfxHitPrefab; // assign your VFX prefab in the inspector

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger detected with " + other.gameObject.name); // Log the name of the triggered object

        if (other.gameObject.CompareTag("Weapon"))
        {
            Debug.Log("COLLIDE");
            //Debug.Log("Triggered with Weapon"); // Log when the triggered object is a Weapon

            // Cast a ray from the sword to the object being hit
            RaycastHit hit;
            if (Physics.Raycast(transform.position, (other.transform.position - transform.position).normalized, out hit))
            {
                // If the ray hits the object, get the point of contact
                Vector3 vfxPosition = hit.point;

                // Calculate an offset that is slightly away from the object, in the direction of the collision normal
                Vector3 offset = hit.normal * 0.1f; // Adjust the multiplier as needed

                GameObject vfxHitInstance = Instantiate(vfxHitPrefab, vfxPosition, Quaternion.identity);
                VisualEffect vfxHit = vfxHitInstance.GetComponentInChildren<VisualEffect>();

                vfxHit.Play();
            }
        }
    }


}
