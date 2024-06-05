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
                _notif.GetComponent<ItemPickupNotif>().Set((itemNotifData)data,2f);
            }
        }
        
    }

    public class itemNotifData
    {
        public string itemName;
        public int quantity;
        public itemNotifData(string _name, int quant)
        {
            itemName = _name;
            quantity = quant;
        }
    }
}
