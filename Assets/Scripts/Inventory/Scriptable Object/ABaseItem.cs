using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Inventory{
    public abstract class ABaseItem : ScriptableObject
    {
        public string itemName;
        public float itemWeight;
        public string itemDescription;
        public Sprite itemSprite;
        public int maxStacks;

        public abstract ItemType GetItemType();
        public abstract void UseItem();
    }

    public enum ItemType
    {
        Materials,
        Consumable,
        Rune,
        Equipment
    }
}
