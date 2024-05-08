using CC.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Items
{

    [CreateAssetMenu(fileName = "ConsumableItem", menuName = "Items/Consumable/Regeneration HP", order = 0)]
    public class RegenerationHP : ConsumableItem
    {
        [Space][Header("Item Effect")]
        public float HealthPointRegenerationAmount;
        public float durationEffect;

        public override float GetAmount()
        {
            return HealthPointRegenerationAmount;
        }
        public override float DurationEffect()
        {
            return durationEffect;
        }

        public override ConsumableType GetConsumableType()
        {
            return ConsumableType.HPRegeneration;
        }
    }
}
