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

    // Call this function when a hit is detected
    public void OnHit()
    {
        StartCoroutine(DoHitPause());
    }

    private IEnumerator DoHitPause()
    {
        Time.timeScale = 0f; // Pause the game

        float pauseEndTime = Time.realtimeSinceStartup + pauseDuration;

        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return null; // Wait for the next frame
        }

        Time.timeScale = 1f; // Resume the game
    }
}
}

