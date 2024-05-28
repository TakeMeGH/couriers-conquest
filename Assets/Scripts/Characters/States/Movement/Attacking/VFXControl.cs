/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace CC.Combats
{
    public class VFXControl : MonoBehaviour
{
     public VisualEffect vfxSlash; // Assign your VFX in the inspector

    void Update()
    {
        if (Time.timeScale == 0f)
        {
            vfxSlash.playRate = 0.0001f; // Slow down the VFX to a near pause
        }
        else
        {
            vfxSlash.playRate = 1f; // Resume the VFX at normal speed
        }
    }

    // Call this method from your collision detection or animation event
    public void PlayVFX()
    {
        if (Time.timeScale != 0f)
        {
            vfxSlash.SendEvent("OnPlay");
        }
    }
}
}
*/