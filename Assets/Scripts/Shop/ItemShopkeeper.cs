using CC.Inventory;
using CC.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Shop
{
    [CreateAssetMenu(fileName = "InventoryData", menuName = "Inventory/Item Shopkeeper", order = 1)]
    public class ItemShopkeeper : DescriptionBaseSO
    {
        public List<ItemSlotInfo> listItem = new List<ItemSlotInfo>();
    }
}
