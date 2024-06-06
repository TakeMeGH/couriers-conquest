using CC.Core.Data.Dynamic;
using CC.Core.Data.Stable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Inventory
{
    [CreateAssetMenu(fileName = "Consumbale", menuName = "Items/Consumable", order = 6)]
    public class ConsumableItem : ABaseItem
    {
        public float durationEffect;
        public StatsModifier effectStats;
        [SerializeField] private ConsumableType _consumableType;

        public override ItemType GetItemType()
        {
            return ItemType.Consumable;
        }

        public override void UseItem()
        {
            Debug.Log("Use Consumbale");
        }

        public float DurationEffect()
        {
            return durationEffect;
        }

        public float GetAmount(mainStat key)
        {
            return effectStats.statsToModify[key];
        }

        public ConsumableType GetConsumableType()
        {
            return _consumableType;
        }
    }
}
