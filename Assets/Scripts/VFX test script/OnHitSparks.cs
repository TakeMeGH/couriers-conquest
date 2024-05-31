using UnityEngine;
using UnityEngine.VFX;

public class OnHitSparks : MonoBehaviour
{
    public void OnHit(Vector3 otherPosition, GameObject otherVFX)
    {
        Debug.Log(otherPosition + " OTHER");
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (otherPosition - transform.position).normalized, out hit))
        {
            Vector3 vfxPosition = hit.point;

            // // Calculate an offset that is slightly away from the object, in the direction of the collision normal
            Vector3 offset = hit.normal * 0.1f; // Adjust the multiplier as needed

            GameObject vfxHitInstance = Instantiate(otherVFX, vfxPosition + offset, Quaternion.identity);
            VisualEffect vfxHit = vfxHitInstance.GetComponentInChildren<VisualEffect>();

            vfxHit.Play();
        }
    }
}