using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Inventory
{
    public abstract class AInventoryData : ScriptableObject
    {
        [SerializeReference] private List<ItemSlotInfo> _items = new List<ItemSlotInfo>();

        [Space]
        public int inventorySize = 28;

        public List<ItemSlotInfo> items { get => _items; set => _items = value; }
    }
}
