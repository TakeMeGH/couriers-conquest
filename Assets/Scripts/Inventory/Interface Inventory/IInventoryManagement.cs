using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Inventory
{
    public interface IInventoryManagement
    {
        public void Initialize(InventoryData inventoryData, IInventoryManager inventoryManager, ItemSlotMouse itemSlotMouse);
        public int OnAddItem(ABaseItem item, int amount);
        public void OnEquipRune();
        public void OnRemoveItem(Component _component, object _item);

        public void OnSellItem(ABaseItem itemSell, int amount);
        public void OnUpdateCurrency(int amount);
    }
}
