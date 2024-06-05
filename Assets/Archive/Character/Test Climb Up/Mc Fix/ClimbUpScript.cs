using CC.Events;
using UnityEngine;

namespace CC
{

    public class ClimbUpScript : MonoBehaviour
    {
        [SerializeField] Transform _targetPos;
        [SerializeField] VoidEventChannelSO _triggerOnMovementStateAnimationTransitionEvent;
        Vector3 _offsetPos;

        private void OnEnable()
        {
            _triggerOnMovementStateAnimationTransitionEvent.OnEventRaised += AddOffset;
        }

        private void OnDisable()
        {
            _triggerOnMovementStateAnimationTransitionEvent.OnEventRaised -= AddOffset;

        }
        void Start()
        {
            _offsetPos = _targetPos.position - transform.position;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void AddOffset()
        {
            transform.position += _offsetPos;
        }


    }
}
