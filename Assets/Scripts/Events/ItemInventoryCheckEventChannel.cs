using CC.Inventory;
using UnityEngine;
using System;

/// <summary>
/// This class is intended for communication with the inventory manager regarding items.
/// </summary>
/// 
namespace CC.Events
{
    [CreateAssetMenu(menuName = "Game/Events/Item Inventory Check Channel")]
    public class ItemInventoryCheckEventChannel : DescriptionBaseSO
    {
        public Func<ABaseItem, bool> OnEventRaised;

        public bool RaiseEvent(ABaseItem _item)
        {
            if (OnEventRaised != null)
                return OnEventRaised.Invoke(_item);
            else
                return false;
        }
    }

}
