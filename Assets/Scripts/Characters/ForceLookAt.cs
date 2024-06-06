using System.Collections;
using System.Collections.Generic;
using CC.Characters;
using UnityEngine;
using CC.Characters.States;


namespace CC
{
    public class ForceLookAt : MonoBehaviour
    {
        [SerializeField] GameObject usedObject;
        [SerializeField] PlayerControllerStatesMachine _playerStates;
        [SerializeField] float _rotationTime = 0.2f;
        Quaternion _initialRotation;
        Quaternion _targetRotation;

        float _currentRotationTime = 100;

        public void Force(Vector3 target)
        {
            if(_playerStates == null) return;
            if (_playerStates.GetCurrentState().GetType() != typeof(PlayerBlockingState)) return;

            _initialRotation = usedObject.transform.rotation;

            Vector3 targetPosition = new Vector3(target.x, usedObject.transform.position.y, target.z);

            _targetRotation = Quaternion.LookRotation(targetPosition - usedObject.transform.position);
            _currentRotationTime = 0f;
        }
        public void UpdateRotation()
        {
            if (_currentRotationTime < _rotationTime)
            {
                _currentRotationTime += Time.deltaTime;
                usedObject.transform.rotation = Quaternion.Slerp(_initialRotation,
                    _targetRotation,
                    Mathf.Min(1f, _currentRotationTime / _rotationTime));
            }
        }

        private void Update()
        {
            UpdateRotation();
        }


    }
}
