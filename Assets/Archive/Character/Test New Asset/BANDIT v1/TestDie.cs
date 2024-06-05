using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC
{
    public class TestDie : MonoBehaviour
    {
        private void Start()
        {
            Die();
        }
        public void Die()
        {
            GetComponent<Animator>().enabled = false; // Disable the animator
            GetComponent<Collider>().enabled = false; // Disable the main collider
            GetComponent<Rigidbody>().isKinematic = true; // Disable the main Rigidbody

            foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
            {
                rb.isKinematic = false; // Enable all child Rigidbody components to activate ragdoll
            }

            foreach (Collider col in GetComponentsInChildren<Collider>())
            {
                col.enabled = true; // Enable all child Collider components
            }

        }
    }
}
