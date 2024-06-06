using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC
{
    public class NPCAnimatorController : MonoBehaviour
    {
        [SerializeField] Animator _animator;

        public void Interact()
        {
            _animator.Play("Interact");
        }
    }
}
