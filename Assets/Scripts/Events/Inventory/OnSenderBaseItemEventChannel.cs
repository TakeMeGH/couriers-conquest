using CC.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CC.UpgradeEquipment
{
    [CreateAssetMenu(menuName = "Game/Events/Base Item Sender") ]
    public class OnSenderBaseItemEventChannel : DescriptionBaseSO
    {
        public UnityAction<ABaseItem> OnEventRaised;

        public void RaiseEvent(ABaseItem items)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(items);
        }
    }
}
