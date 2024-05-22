using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CC.Inventory
{
    public interface IInventoryAction
    {
        public void Initialize(InventoryData inventoryData, IInventoryManager playerInventoryManager, ItemSlotMouse itemSlotMouse, Slider sliderDrop);
        public void OnDropItem(int index, int amount);
        public bool CheckItem(ABaseItem _item);

        public void UpgradeItem(ABaseItem _item);
    }
}
