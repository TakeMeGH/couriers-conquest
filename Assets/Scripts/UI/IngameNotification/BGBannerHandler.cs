using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CC.UI.Notification
{
    public class BGBannerHandler : MonoBehaviour
    {
        [SerializeField] Image _BG;
        [SerializeField] TextMeshProUGUI _title;
        [SerializeField] TextMeshProUGUI _description;
        [SerializeField] float _time = 3f;
        public void Set(BGBannerData _data)
        {
            if (_data.BG != null) _BG.sprite = _data.BG;
            _title.text = _data.Title;
            _description.text = "-" + _data.description + "-";
        }

        private void Update()
        {
            if (_time > 0) _time -= Time.deltaTime;
            if (_time <= 0) Destroy(gameObject);
        }
    }
}
