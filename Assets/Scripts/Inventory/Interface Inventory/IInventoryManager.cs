using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Inventory
{
    public interface IInventoryManager
    {
        public void RefreshInventory();
        public void ClearSlot(ItemSlotInfo slot);
    }
}
