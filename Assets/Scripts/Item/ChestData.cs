using CC.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Items
{
    [CreateAssetMenu(fileName = "Chest", menuName = "Items/Chest", order = 7)]
    public class ChestData : ScriptableObject
    {
        public List<ChestValue> itemChest = new List<ChestValue>();
        public bool isColectable;
    }

    [System.Serializable]
    public class ChestValue
    {
        public ABaseItem item;
        public int amount;
    }
}
