using UnityEngine;
using UnityEngine.VFX;

public class WeaponSlash : MonoBehaviour
{
    public VisualEffect vfxSlash1; // Assign your VFX VisualEffect for slash1 in the inspector
    public VisualEffect vfxSlash2; // Assign your VFX VisualEffect for slash2 in the inspector
    public VisualEffect vfxSlash3; // Assign your VFX VisualEffect for slash3 in the inspector

    private void Update()
    {
        // Check if the game is paused
        if (Time.timeScale == 0f)
        {
            // Stop all VFX if the game is paused
            vfxSlash1.Stop();
            vfxSlash2.Stop();
            vfxSlash3.Stop();
        }
    }

    // These methods would be called at the appropriate time in your sword swing animations
    public void TriggerVFXSlash1()
    {
        vfxSlash1.Play();
    }

    public void TriggerVFXSlash2()
    {
        vfxSlash2.Play();
    }

    public void TriggerVFXSlash3()
    {
        vfxSlash3.Play();
    }
}
