using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cc_inventory
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Items/Material", order = 1)]
    public class MaterialItem : ABaseItem
    {
        public float costSell;
        public override ItemType GetItemType()
        {
            return ItemType.Materials;
        }
    }
}
