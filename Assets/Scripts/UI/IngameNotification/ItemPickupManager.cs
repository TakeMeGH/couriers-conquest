using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.UI.Notification
{
    public class ItemPickupManager : MonoBehaviour
    {
        [SerializeField] GameObject _itemPickupNotif;
        [SerializeField] GameObject _itemPickupLayout;
        
        public void spawnNotif(Component sender, object data)
        {
            if(data is itemNotifData)
            {
                GameObject _notif = Instantiate(_itemPickupNotif, _itemPickupLayout.transform);
                _notif.GetComponent<ItemPickupNotif>().Set((itemNotifData)data,2.5f);
            }
        }
        
    }

    public class itemNotifData
    {
        public Sprite icon;
        public string itemName;
        public int quantity;
        public itemNotifData(string _name, int quant, Sprite _icon)
        {
            itemName = _name;
            quantity = quant;
            icon = _icon;
        }
    }
}
