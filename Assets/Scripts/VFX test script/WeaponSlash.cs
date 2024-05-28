using UnityEngine;
using UnityEngine.VFX;

public class SwordSwing : MonoBehaviour
{
    public VisualEffect vfxSlash1; // assign your VFX VisualEffect for slash1 in the inspector
    public VisualEffect vfxSlash2; // assign your VFX VisualEffect for slash2 in the inspector
    public VisualEffect vfxSlash3; // assign your VFX VisualEffect for slash2 in the inspector

    // These methods would be called at the appropriate time in your sword swing animations
    public void TriggerVFXSlash1()
    {
        vfxSlash1.SendEvent("OnPlay");
        Debug.Log("Test vfx");
    }

    public void TriggerVFXSlash2()
    {
        vfxSlash2.SendEvent("OnPlay");
    }
     public void TriggerVFXSlash3()
    {
        vfxSlash3.SendEvent("OnPlay");
    }
}
