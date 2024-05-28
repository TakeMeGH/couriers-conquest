using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CC.UI.Notification
{
    public class textBannerHandler : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _text;
        [SerializeField] float _time = 3f;
        public void Set(string text)
        {
            _text.text = "-" + text + "-";
        }

        private void Update()
        {
            if (_time > 0) _time -= Time.deltaTime;
            if (_time <= 0) Destroy(this.gameObject);
        }
    }
}
