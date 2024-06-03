using UnityEngine;
using UnityEngine.VFX;
using CC.Characters;

namespace CC.Characters.States
{
    public class ShieldVFX : MonoBehaviour
    {
        public VisualEffect shieldVFX; // Assign your shield VFX in the inspector

        // Call this method to start the shield VFX
        // These methods would be called at the appropriate time in your sword swing animations
    public void TriggerVFXBlock()
    {
        shieldVFX.Play();
    }

    public void ReinitializeVFX()
    {
        shieldVFX.Reinit();
    } 
    }
}
