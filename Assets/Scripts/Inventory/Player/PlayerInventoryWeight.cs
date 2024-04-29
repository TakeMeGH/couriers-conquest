using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Inventory
{
    public class PlayerInventoryWeight : IInventoryWeight
    {
        private InventoryData _inventoryData;
        private float _totalWeight;

        public void Initialize(InventoryData inventoryData)
        {
            _inventoryData = inventoryData;
        }

        public float GetWeight()
        {
            _totalWeight = 0;
            for (int i = 0; i < _inventoryData.items.Count; i++)
            {
                if (_inventoryData.items[i].item != null)
                {
                    _totalWeight += _inventoryData.items[i].item.itemWeight * _inventoryData.items[i].stacks;
                }
            }
            _inventoryData.onWeightUpdated.RaiseEvent(_totalWeight);

            return _totalWeight;
        }

    }
}
