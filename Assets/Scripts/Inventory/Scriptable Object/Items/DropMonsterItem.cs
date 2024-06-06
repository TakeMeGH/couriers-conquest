using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Inventory
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Items/Drop Monster", order = 1)]
    public class DropMonsterItem : ABaseItem
    {
        public override ItemType GetItemType()
        {
            return ItemType.DropMonster;
        }

        public override void UseItem()
        {
            Debug.Log("Use " + itemName);
        }
    }
}
