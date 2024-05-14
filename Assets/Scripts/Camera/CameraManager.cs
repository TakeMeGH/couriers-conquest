using Cinemachine;
using UnityEngine;
using CC.RuntimeAnchors;
using CC.Events;

namespace CC.Camera
{
    public class CameraManager : MonoBehaviour
    {

        [SerializeField] TransformAnchor _playerTransformAnchor = default;
        [SerializeField] CinemachineVirtualCamera _playerCamera;
        [SerializeField] VoidEventChannelSO _disableCameraInputEvent;
        [SerializeField] VoidEventChannelSO _enableCameraInputEvent;
        CinemachineInputProvider _cameraControlAction;  // Assign this in the Inspector


        private void OnEnable()
        {
            _playerTransformAnchor.OnAnchorProvided += SetupPlayerVirtualCamera;
            _enableCameraInputEvent.OnEventRaised += EnableCameraControl;
            _disableCameraInputEvent.OnEventRaised += DisableCameraControl;

        }

        private void OnDisable()
        {
            _playerTransformAnchor.OnAnchorProvided -= SetupPlayerVirtualCamera;
            _enableCameraInputEvent.OnEventRaised -= EnableCameraControl;
            _disableCameraInputEvent.OnEventRaised -= DisableCameraControl;
        }

        private void Awake()
        {
            _cameraControlAction = _playerCamera.GetComponent<CinemachineInputProvider>();
        }

        public void SetupPlayerVirtualCamera()
        {
            Transform target = _playerTransformAnchor.Value;

            _playerCamera.Follow = target;
            _playerCamera.LookAt = target;
        }

        public void EnableCameraControl()
        {
            if (_cameraControlAction != null)
            {
                _playerCamera.enabled = true;
                _cameraControlAction.enabled = true;
            }
        }

        public void DisableCameraControl()
        {
            if (_cameraControlAction != null)
            {
                _playerCamera.enabled = false;
                _cameraControlAction.enabled = false;
            }
        }



    }

}
