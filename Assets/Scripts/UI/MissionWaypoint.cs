using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CC.RuntimeAnchors;


namespace CC
{
    public class MissionWaypoint : MonoBehaviour
    {
        public Image img;
        public Transform target;
        public Vector3 offset;
        public TMP_Text meter;
        public float offsetWidth;
        public float offsetHeight;

        [SerializeField] TransformAnchor _playerTransformAnchor = default;
        Transform _playerTransform;
        [SerializeField] private float _moveSpeed = 5.0f;
        private void OnEnable()
        {
            _playerTransformAnchor.OnAnchorProvided += SetPlayerTransfrom;
        }

        private void OnDisable()
        {
            _playerTransformAnchor.OnAnchorProvided -= SetPlayerTransfrom;
        }


        private void Update()
        {
            if (_playerTransform != null)
            {
                transform.position = _playerTransform.position;
                transform.rotation = UnityEngine.Camera.main.gameObject.transform.rotation;

            }
            float minX = img.GetPixelAdjustedRect().width / 2;
            float maxX = Screen.width - minX;
            maxX -= offsetWidth;
            minX += offsetWidth;

            float minY = img.GetPixelAdjustedRect().height / 2;
            float maxY = Screen.height - minY;
            maxY -= offsetHeight;
            minY += offsetHeight;


            Vector2 pos = UnityEngine.Camera.main.WorldToScreenPoint(target.position + offset);

            if (Vector3.Dot(target.position - transform.position, transform.forward) < 0)
            {
                if (pos.x < Screen.width / 2)
                {
                    pos.x = maxX;
                }
                else
                {
                    pos.x = minX;
                }
            }

            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);

            // Vector2 distance = pos - (Vector2)img.transform.position;
            // Vector2 addDistance = distance.normalized * Time.deltaTime * _moveSpeed;
            // // Debug.Log(distance + " TEST " + addDistance + " " + pos);
            // if (distance.magnitude > addDistance.magnitude)
            // {
            //     img.transform.position = (Vector2)img.transform.position + addDistance;
            // }
            // else
            // {
            //     img.transform.position = pos;
            // }
            img.transform.position = pos;
            meter.text = ((int)Vector3.Distance(target.position, transform.position)).ToString() + "M";
        }
        void SetPlayerTransfrom()
        {
            _playerTransform = _playerTransformAnchor.Value;
        }

    }

}
