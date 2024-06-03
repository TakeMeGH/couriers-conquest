using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CC.RuntimeAnchors;


namespace CC
{
    public class MissionWaypoint : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset;
        public float offsetWidth;
        public float offsetHeight;

        [SerializeField] TransformAnchor _playerTransformAnchor = default;
        [SerializeField] float _showTime;
        [SerializeField] InputReader _inputReader;
        bool _isShowing = false;
        float _currentShowTime;
        Transform _playerTransform;
        Image img;
        TMP_Text meter;

        private void OnEnable()
        {
            _playerTransformAnchor.OnAnchorProvided += SetPlayerTransfrom;
            _inputReader.ShowWaypointPerformed += ActivateHint;
        }

        private void OnDisable()
        {
            _playerTransformAnchor.OnAnchorProvided -= SetPlayerTransfrom;
            _inputReader.ShowWaypointPerformed -= ActivateHint;
        }


        private void Start()
        {
            if (img == null) FindImage();

            if(img != null && !_isShowing) stopHint();
        }

        void FindImage()
        {
            GameObject _waypointIcon = GameObject.FindGameObjectWithTag("WaypointIcon");
            if (_waypointIcon != null)
            {
                img = _waypointIcon.GetComponent<Image>();
                meter = _waypointIcon.GetComponentInChildren<TMP_Text>();
            }
            Debug.Log("START " + img + " " + meter);

        }
        public void Hints(Component sender, object data)
        {
            if (data is HintData)
            {
                HintData _data = (HintData)data;
                target = _data.Destination.transform;
                ActivateHint();
            }
        }
        void ActivateHint()
        {
            if (target == null) return;
            if (img == null) FindImage();
            _isShowing = true;
            img.gameObject.SetActive(true);
            _currentShowTime = _showTime;

        }

        public void stopHint()
        {
            _isShowing = false;
            img.gameObject.SetActive(false);
            target = null;
        }
        private void Update()
        {
            if (!_isShowing) return;
            if (target == null) return;

            _currentShowTime -= Time.deltaTime;

            if (_currentShowTime <= 0)
            {
                img.gameObject.SetActive(false);
            }

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

            img.transform.position = pos;
            meter.text = ((int)Vector3.Distance(target.position, transform.position)).ToString() + "M";
        }
        void SetPlayerTransfrom()
        {
            _playerTransform = _playerTransformAnchor.Value;
        }

    }

}
