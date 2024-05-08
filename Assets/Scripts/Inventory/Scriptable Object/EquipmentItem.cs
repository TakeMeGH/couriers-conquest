using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using CC.Core.Data.Dynamic;
using UnityEngine;

namespace CC.Inventory
{
    [CreateAssetMenu(fileName = "EquipmentItem", menuName = "Items/Equipment", order = 3)]
    public class EquipmentItem : ABaseItem
    {
        public int equipmentLevel;
        //public float attackWeapon;
        //public float healthWeapon;
        public float costSell;
        //public float shieldDamageReduction;
        public SerializedDictionary<mainStat, float> EquipmentStats;
        public ItemSlotType specificType;

        public override ItemType GetItemType()
        {
            return ItemType.Equipment;
        }

        public override void UseItem()
        {
            Debug.Log("Use : " + itemName);
        }
    }
}
