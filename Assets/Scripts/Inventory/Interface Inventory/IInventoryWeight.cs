using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Inventory
{
    public interface IInventoryWeight
    {
        public void Initialize(InventoryData inventoryData);
        public float GetWeight();
    }
}
