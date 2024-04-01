using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Inventory{
    public abstract class ABaseItem : ScriptableObject
    {
        public string itemName;
        public string itemId;
        public float itemWeight;
        public string itemDescription;
        public Sprite itemSprite;
        public int maxStacks;
        public GameObject itemPrefab;

        public abstract ItemType GetItemType();

    }

    public enum ItemType
    {
        Materials,
        Potion,
        Weapon,
        Rune
    }
}
