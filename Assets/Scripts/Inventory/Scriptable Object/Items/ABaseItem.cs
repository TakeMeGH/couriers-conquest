using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Inventory
{
    public abstract class ABaseItem : ScriptableObject
    {
        public string itemName;
        public string idItem;
        public float itemWeight;
        [TextArea] public string itemDescription;
        public Sprite itemSprite;
        public int maxStacks;
        public int costBuy;
        public int costSell;

        public abstract ItemType GetItemType();
        public abstract void UseItem();
    }

}
