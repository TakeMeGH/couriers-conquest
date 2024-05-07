using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Inventory
{
    [CreateAssetMenu(menuName = "Game/Events/Buy Item Channel")]
    public class BuyItemEventChannel : DescriptionBaseSO
    {
        public Action<List<ABaseItem>, List<int>> OnEventRaised;

        public void RaiseEvent(List<ABaseItem> items, List<int> Amount)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(items, Amount);
        }
    }
}
