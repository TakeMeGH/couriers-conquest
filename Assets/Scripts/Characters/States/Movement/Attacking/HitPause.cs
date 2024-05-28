using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using CC.Characters;

namespace CC.Combats
{
   public class HitPause : MonoBehaviour
{
    public float pauseDuration = 0.1f; // Duration of the hit pause
    public VisualEffect[] visualEffects; // Array of VisualEffect components to pause and resume

    // Call this function when a hit is detected
    public void OnHit()
    {
        StartCoroutine(DoHitPause());
    }

    private IEnumerator DoHitPause()
    {
        Time.timeScale = 0f; // Pause the game

        // "Pause" the visual effects
        foreach (var vfx in visualEffects)
        {
            vfx.playRate = 0.0001f; // Set the playback speed to nearly zero
        }

        float pauseEndTime = Time.realtimeSinceStartup + pauseDuration;

        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return null; // Wait for the next frame
        }

        Time.timeScale = 1f; // Resume the game

        // "Resume" the visual effects
        foreach (var vfx in visualEffects)
        {
            vfx.playRate = 1f; // Set the playback speed back to normal
        }
    }
}
}

