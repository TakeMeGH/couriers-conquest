using CC.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Events
{
    [CreateAssetMenu(menuName = "Game/Events/Update Currency")]
    public class OnUpdateCurrencyEventChannel : DescriptionBaseSO
    {
        public Action<int> OnEventRaised;

        public void RaiseEvent(int _amount)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(_amount);
        }
    }
}
