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
        public SerializedDictionary<mainStat, float> _baseStats;
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

        public float GetStatsWeapon(mainStat key)
        {
            if (equipmentLevel <= 0)
            {
                if (_baseStats.TryGetValue(key, out float value))
                {
                    return value;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                if (upgradeRequiriment[equipmentLevel - 1].EquipmentStats.TryGetValue(key, out float value))
                {
                    return value;
                }
                else
                {
                    return 0;
                }
            }
        }
    }

    [System.Serializable]
    public class UpgradeRequiriment
    {
        public List<UpgradeMaterialRequiriment> materialRequiriment = new List<UpgradeMaterialRequiriment>();
        public int price;
        public SerializedDictionary<mainStat, float> EquipmentStats;

        public float GetStatsUpgradeWeapon(mainStat key)
        {
            if (EquipmentStats.TryGetValue(key, out float value))
            {
                return value;
            }
            else
            {
                return 0;
            }
        }
    }

    [System.Serializable]
    public class UpgradeMaterialRequiriment
    {
        public ABaseItem itemRequiriment;
        public int amount;
    }
}
