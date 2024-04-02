using Cinemachine;
using UnityEngine;
using CC.RuntimeAnchors;

namespace CC.Camera
{
    public class CameraManager : MonoBehaviour
    {

        [SerializeField] TransformAnchor _playerTransformAnchor = default;
        [SerializeField] CinemachineVirtualCamera _playerCamera;

        private void OnEnable()
        {
            _playerTransformAnchor.OnAnchorProvided += SetupPlayerVirtualCamera;

        }

        private void OnDisable()
        {
            _playerTransformAnchor.OnAnchorProvided += SetupPlayerVirtualCamera;

        }

        public void SetupPlayerVirtualCamera()
        {
            Transform target = _playerTransformAnchor.Value;

            _playerCamera.Follow = target;
            _playerCamera.LookAt = target;
        }

    }

}
