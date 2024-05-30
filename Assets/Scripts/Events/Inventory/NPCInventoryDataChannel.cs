using CC.Inventory;
using CC.Shop;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Events
{
    [CreateAssetMenu(menuName = "Game/Events/Shopkeeper Data Channel")]
    public class NPCInventoryDataChannel : DescriptionBaseSO
    {
        public Action<ItemShopkeeper> OnEventRaised;

        public void RaiseEvent(ItemShopkeeper _shopkeeperInventory)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(_shopkeeperInventory);
        }
    }
}
