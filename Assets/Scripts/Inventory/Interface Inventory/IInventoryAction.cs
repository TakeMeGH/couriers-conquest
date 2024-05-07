using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Inventory
{
    public interface IInventoryAction
    {
        public void Initialize(InventoryData inventoryData, IInventoryManager playerInventoryManager, ItemSlotMouse itemSlotMouse);
        public void OnDropItem();
        public bool CheckItem(ABaseItem _item);

        public void UpgradeItem(ABaseItem _item);
    }
}
