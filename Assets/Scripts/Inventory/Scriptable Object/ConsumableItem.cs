using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Inventory{

    [CreateAssetMenu(fileName = "ItemData", menuName = "Items/Potion", order = 2)]
    public class ConsumableItem : ABaseItem
    {
        public float bonusHp;
        public float bonusAttack;
        public float bonusMagic;
        public float bonusDeff;

        public override ItemType GetItemType()
        {
            return ItemType.Consumable;
        }

        public override void UseItem()
        {
            Debug.Log("Use Consumbale");
        }
    }
}
