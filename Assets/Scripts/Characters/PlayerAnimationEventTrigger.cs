using CC.Events;
using UnityEngine;

namespace CC.Characters
{
    public class PlayerAnimationEventTrigger : MonoBehaviour
    {
        Animator _animator;
        [SerializeField] VoidEventChannelSO _triggerOnMovementStateAnimationEnterEvent;
        [SerializeField] VoidEventChannelSO _triggerOnMovementStateAnimationExitEvent;
        [SerializeField] VoidEventChannelSO _triggerOnMovementStateAnimationTransitionEvent;

        private void Awake() {
            _animator = GetComponent<Animator>();
        }
        public void TriggerOnMovementStateAnimationEnterEvent()
        {
            if (IsInAnimationTransition())
            {
                return;
            }

            _triggerOnMovementStateAnimationEnterEvent.RaiseEvent();
        }

        public void TriggerOnMovementStateAnimationExitEvent()
        {
            if (IsInAnimationTransition())
            {
                return;
            }

            _triggerOnMovementStateAnimationExitEvent.RaiseEvent();
        }

        public void TriggerOnMovementStateAnimationTransitionEvent()
        {
            if (IsInAnimationTransition())
            {
                return;
            }

            _triggerOnMovementStateAnimationTransitionEvent.RaiseEvent();
        }

        private bool IsInAnimationTransition(int layerIndex = 0)
        {
            return _animator.IsInTransition(layerIndex);
        }
    }
}