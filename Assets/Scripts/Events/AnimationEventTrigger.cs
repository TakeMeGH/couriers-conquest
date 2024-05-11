using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CC.Event
{
    public class AnimationEventTrigger : MonoBehaviour
    {
        Animator _animator;
        [SerializeField] UnityEvent _triggerOnMovementStateAnimationEnterEvent;
        [SerializeField] UnityEvent _triggerOnMovementStateAnimationExitEvent;
        [SerializeField] UnityEvent _triggerOnMovementStateAnimationTransitionEvent;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        public void TriggerOnMovementStateAnimationEnterEvent()
        {
            if (IsInAnimationTransition())
            {
                return;
            }

            _triggerOnMovementStateAnimationEnterEvent.Invoke();
        }

        public void TriggerOnMovementStateAnimationExitEvent()
        {
            if (IsInAnimationTransition())
            {
                return;
            }

            _triggerOnMovementStateAnimationExitEvent.Invoke();
        }

        public void TriggerOnMovementStateAnimationTransitionEvent()
        {
            if (IsInAnimationTransition())
            {
                return;
            }

            _triggerOnMovementStateAnimationTransitionEvent.Invoke();
        }

        private bool IsInAnimationTransition(int layerIndex = 0)
        {
            return _animator.IsInTransition(layerIndex);
        }
    }
}
