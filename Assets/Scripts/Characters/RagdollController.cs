using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

namespace CC.Ragdoll
{
    public class RagdollController : MonoBehaviour
    {
        [SerializeField] Animator _animator;
        [SerializeField] LayerMask includeLayer;
        [SerializeField] LayerMask excludeLayer;


        Rigidbody[] rigidbodies;
        Collider[] colliders;
        void Awake()
        {
            rigidbodies = GetComponentsInChildren<Rigidbody>();
            colliders = GetComponentsInChildren<Collider>();

            SetRagdoll(false, true, true);

            // Testing Purposes
            // SetRagdoll(true, false);
        }

        public void SetRagdoll(bool state, bool animatorState, bool isFirstTime = false)
        {
            foreach (var rb in rigidbodies)
            {
                rb.isKinematic = !state;
                if (isFirstTime)
                {
                    rb.includeLayers = includeLayer;
                    rb.excludeLayers = excludeLayer;
                }
            }

            foreach (var col in colliders)
            {
                col.enabled = state;
            }

            _animator.enabled = animatorState;
        }
    }
}
