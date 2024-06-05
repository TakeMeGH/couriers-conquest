using CC.RuntimeAnchors;
using Unity.Mathematics;
using UnityEngine;

namespace CC
{
    public class MiniMapController : MonoBehaviour
    {
        [SerializeField] TransformAnchor _playerTransformAnchor = default;
        [SerializeField] Vector3 _targetOffset;
        [SerializeField] Vector3 _cameraRotation;

        Transform _target;

        private void OnEnable()
        {
            _playerTransformAnchor.OnAnchorProvided += SetupPlayerVirtualCamera;

        }

        private void OnDisable()
        {
            _playerTransformAnchor.OnAnchorProvided -= SetupPlayerVirtualCamera;
        }

        public void SetupPlayerVirtualCamera()
        {
            _target = _playerTransformAnchor.Value;
        }

        private void Update()
        {
            if (_target != null)
            {
                transform.position = new Vector3(_target.position.x + _targetOffset.x,
                    _target.position.y + _targetOffset.y,
                    _target.position.z + _targetOffset.z);
                    
                transform.rotation = Quaternion.Euler(_cameraRotation.x, 
                    _cameraRotation.y, _cameraRotation.z);
            }
        }

    }
}
