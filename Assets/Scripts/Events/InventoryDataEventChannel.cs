using CC.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Events
{
    [CreateAssetMenu(menuName = "Game/Events/Inventory Data Channel")]
    public class InventoryDataEventChannel : DescriptionBaseSO
    {
        public Action<AInventoryData> OnEventRaised;

        public void RaiseEvent(AInventoryData _inventoryData)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(_inventoryData);
        }
    }
}
