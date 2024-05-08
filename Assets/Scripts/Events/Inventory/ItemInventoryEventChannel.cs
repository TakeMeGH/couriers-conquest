using CC.Inventory;
using UnityEngine;
using System;

/// <summary>
/// This class is intended for communication with the inventory manager regarding items.
/// </summary>
/// 
namespace CC.Events
{
    [CreateAssetMenu(menuName = "Game/Events/Item Inventory Channel")]
    public class ItemInventoryEventChannel : DescriptionBaseSO
    {
        public Func<ABaseItem, int, int> OnEventRaised;

        public int RaiseEvent(ABaseItem _item, int _amount)
        {
            if (OnEventRaised != null)
                return OnEventRaised.Invoke(_item, _amount);
            else
                return 0;
        }
    }

}
