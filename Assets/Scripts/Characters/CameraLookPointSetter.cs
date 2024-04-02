using UnityEngine;
using CC.RuntimeAnchors;


namespace CC.Characters
{
    public class CameraLookPointSetter : MonoBehaviour
    {
        [SerializeField] TransformAnchor _playerCameraLookTransform;
        private void OnEnable()
        {
            _playerCameraLookTransform.Provide(transform);
        }

        private void OnDisable()
        {
            _playerCameraLookTransform.Unset();

        }
    }

}