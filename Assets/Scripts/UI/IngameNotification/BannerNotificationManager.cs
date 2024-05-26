using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.UI.Notification
{
    public class BannerNotificationManager : MonoBehaviour
    {
        [Header("Banner Prefab")]
        [SerializeField] GameObject[] _prefabs;
        // 0 : Text; 1 : BG
        [Header("Param")]
        [SerializeField] Transform _layout;

        public void spawnBanner(Component sender, object data)
        {
            if(data is string)
            {

            }
            if(data is BGBannerData)
            {

            }
        }
    }

    public class BGBannerData
    {
        public Sprite BG;
        public string Title;
        public string description;
    }
}
