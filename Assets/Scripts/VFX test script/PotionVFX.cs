using System.Collections;
using System.Collections.Generic;
using CC.Events;
using UnityEngine;
using UnityEngine.VFX;

namespace CC.Inventory
{
    public class PotionVFX : MonoBehaviour
    {
        [SerializeField]
        private VisualEffect healEffect; // Reference to the healing VFX particle system
        [SerializeField]
        private float vfxDuration = 5f;
        [SerializeField] VoidEventChannelSO _enableVFXHeal;

        private void OnEnable()
        {
            _enableVFXHeal.OnEventRaised += ActivateHealingEffect;
        }

        private void OnDisable()
        {
            _enableVFXHeal.OnEventRaised -= ActivateHealingEffect;
        }
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
