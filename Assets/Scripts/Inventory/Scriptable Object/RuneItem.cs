using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Inventory{

    [CreateAssetMenu(fileName = "Rune", menuName = "Items/Rune", order = 4)]
    public class RuneItem : ABaseItem
    {
        public float runeDamage;
        public float costSell;
        public override ItemType GetItemType()
        {
            return ItemType.Rune;
        }
    }
}
