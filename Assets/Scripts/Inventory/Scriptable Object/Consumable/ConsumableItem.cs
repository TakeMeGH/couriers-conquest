using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Inventory
{
    public abstract class ConsumableItem : ABaseItem
    {

        public override ItemType GetItemType()
        {
            return ItemType.Consumable;
        }

        public override void UseItem()
        {
            Debug.Log("Use Consumbale");
        }

        public abstract float GetAmount();
        public abstract float DurationEffect();
        public abstract ConsumableType GetConsumableType();
    }
}
