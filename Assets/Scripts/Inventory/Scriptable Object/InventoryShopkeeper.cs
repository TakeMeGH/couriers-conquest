using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Inventory
{
    [CreateAssetMenu(fileName = "InventoryData", menuName = "Inventory/Shopkeeper", order = 1)]
    public class InventoryShopkeeper : AInventoryData
    {
        [SerializeReference] public List<ABaseItem> sellItem = new List<ABaseItem>();
    }
}
