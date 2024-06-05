using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CC.UI.Notification
{
    public class ItemPickupNotif : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _itemName;
        [SerializeField] TextMeshProUGUI _itemQuantity;
        [SerializeField] float _duration;
        [SerializeField] bool _onScreen;
        public void Set(itemNotifData data, float duration)
        {
            _itemName.text = data.itemName;
            _itemQuantity.text = "x" + data.quantity.ToString();
            _duration = duration;
            _onScreen = true;
        }

        private void Update()
        {
            if (_duration > 0) _duration -= Time.deltaTime;
            if (_onScreen && _duration <= 0) Destroy(gameObject);
        }
    }
}
