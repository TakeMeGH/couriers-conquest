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
        public SerializedDictionary<mainStat, float> EquipmentStats;
        public ItemSlotType specificType;
        public List<UpgradeRequiriment> upgradeRequiriment = new List<UpgradeRequiriment>();

        public override ItemType GetItemType()
        {
            return ItemType.Equipment;
        }

        public override void UseItem()
        {
            Debug.Log("Use : " + itemName);
        }
    }

    [System.Serializable]
    public class UpgradeRequiriment
    {
        public List<UpgradeMaterialRequiriment> materialRequiriment = new List<UpgradeMaterialRequiriment>();
        public float price;
        public SerializedDictionary<mainStat, float> EquipmentStats;
    }

    [System.Serializable]
    public class UpgradeMaterialRequiriment
    {
        public ABaseItem itemRequiriment;
        public int amount;
    }
}
