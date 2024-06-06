using UnityEngine;
using UnityEngine.VFX;

public class EnemyWeaponSlash : MonoBehaviour
{
    public VisualEffect vfxSlash1; // Assign your VFX VisualEffect for slash1 in the inspector
    public VisualEffect vfxSlash2; // Assign your VFX VisualEffect for slash2 in the inspector

    // These methods would be called at the appropriate time in your sword swing animations
    public void TriggerVFXSlash1()
    {
        vfxSlash1.Play();
    }

    public void TriggerVFXSlash2()
    {
        vfxSlash2.Play();
    }
}
