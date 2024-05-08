using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Inventory
{
    [CreateAssetMenu(menuName = "Game/Events/Sell Item Channel")]
    public class SellItemEventChannel : DescriptionBaseSO
    {
        public Action<ABaseItem, int> OnEventRaised;

        public void RaiseEvent(ABaseItem items, int Amount)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(items, Amount);
        }
    }
}
