using CC.Core.Data.Stable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Inventory{

    [CreateAssetMenu(fileName = "Rune", menuName = "Items/Rune", order = 4)]
    public class RuneItem : ABaseItem
    {
        public StatsModifier runeStats;
        public override ItemType GetItemType()
        {
            return ItemType.Rune;
        }

        public override void UseItem()
        {
            Debug.Log("Use Rune");
        }
    }
}
