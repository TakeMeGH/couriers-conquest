using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cc_inventory{

    [CreateAssetMenu(fileName = "ItemData", menuName = "Items/Potion", order = 2)]
    public class PotionItem : ABaseItem
    {
        public float bonusHp;
        public float bonusAttack;
        public float bonusMagic;
        public float bonusDeff;

        public override ItemType GetItemType()
        {
            return ItemType.Potion;
        }
    }
}
