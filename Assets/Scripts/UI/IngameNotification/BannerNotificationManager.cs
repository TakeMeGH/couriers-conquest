using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.UI.Notification
{
    public class BannerNotificationManager : MonoBehaviour
    {
        [Header("Banner Prefab")]
        [SerializeField] GameObject _prefabs;
        [Header("Param")]
        [SerializeField] Transform _layout;

        public void spawnBanner(Component sender, object data)
        {
            if (data is string)
            {
                BGBannerHandler temp = Instantiate(_prefabs, _layout).GetComponent<BGBannerHandler>();
                temp.Set((string)data);
            }
        }
    }
}
