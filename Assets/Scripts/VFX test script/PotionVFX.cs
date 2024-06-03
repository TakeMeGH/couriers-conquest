using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace CC.Inventory
{
    public class PotionVFX : MonoBehaviour
    {
        [SerializeField]
        private VisualEffect healEffect; // Reference to the healing VFX particle system
        [SerializeField]
        private float vfxDuration = 5f; // Duration for the VFX to play before stopping

        // Function to activate the healing effect
        public void ActivateHealingEffect()
        {
            if (healEffect != null)
            {
                healEffect.Play();
                StartCoroutine(DelayedStopVFX(vfxDuration)); // Start the delayed stop coroutine
            }
        }

        // Public function to deactivate the healing effect with a delay
        public void DeactivateHealingEffect()
        {
            if (healEffect != null)
            {
                StartCoroutine(DelayedStopVFX(0)); // Stop the VFX immediately
            }
        }

        // Coroutine to delay the stopping of the VFX
        private IEnumerator DelayedStopVFX(float delay)
        {
            yield return new WaitForSeconds(delay);
            healEffect.Stop(); // Stop the VFX after the delay
        }
    }
}
