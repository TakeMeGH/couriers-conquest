using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

namespace CC.Ragdoll
{
    public class RagdollController : MonoBehaviour
    {
        [SerializeField] Animator _animator;
        Rigidbody[] rigidbodies;
        Collider[] colliders;

        void Awake()
        {
            rigidbodies = GetComponentsInChildren<Rigidbody>();
            colliders = GetComponentsInChildren<Collider>();

            SetRagdoll(false, true);
        }

        public void SetRagdoll(bool state, bool animatorState)
        {
            foreach (var rb in rigidbodies)
            {
                rb.isKinematic = !state;
            }

            foreach (var col in colliders)
            {
                col.enabled = state;
            }

            _animator.enabled = animatorState;
        }
    }
}
