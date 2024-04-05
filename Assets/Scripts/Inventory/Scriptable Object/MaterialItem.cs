using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Inventory
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Items/Material", order = 1)]
    public class MaterialItem : ABaseItem
    {
        public float costSell;
        public override ItemType GetItemType()
        {
            return ItemType.Materials;
        }

        public override void UseItem()
        {
            Debug.Log("Use " + itemName);
        }
    }
}
