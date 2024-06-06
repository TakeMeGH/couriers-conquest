using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CC.UI.Notification
{
    public class BGBannerHandler : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _title;
        public void Set(string questTitle)
        {
            _title.text = questTitle;
        }

        public void DestroyBanner()
        {
            Destroy(gameObject);
        }
    }
}
