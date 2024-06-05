using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace CC.Characters.States
{
    public class SprintVFX : MonoBehaviour
    {
        public VisualEffect sprintVFX; // Assign your shield VFX in the inspector

        // Call this method to start the shield VFX
        // These methods would be called at the appropriate time in your sword swing animations
    public void TriggerVFXSprint()
    {
        sprintVFX.Play();
    }

    public void ReinitializeSprint()
    {
        sprintVFX.Reinit();
    } 
    }
}
