using CC.Core.Save;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using Newtonsoft.Json.Linq;
using CC.Core.Data.Stable;

namespace CC.Core.Data.Dynamic
{
    [CreateAssetMenu(menuName = "Data/Dynamics/PlayerStats")]
    public class PlayerStatsSO : ASavableModel
    {
        [SerializeField] PlayerStats statData;
        [SerializeField] List<StatsModifier> modifiers;
        [Header("default")]
        [SerializeField] PlayerStats _defaultStatData;
        public float GetValue(mainStat key)
        {
            if (statData.defaultValue.TryGetValue(key, out float value))
            {
                foreach (var modifier in modifiers)
                {
                    if (!modifier.statsToModify.ContainsKey(key)) continue;
                    else if (modifier.isPercent) value *= modifier.statsToModify[key];
                    else value += modifier.statsToModify[key];
                }
                return value;
            }
            else
            {
                return 0;
            }
        }

        public float GetInstanceValue(mainStat key)
        {
            if (statData.instanceValue.TryGetValue(key, out float value))
            {
                return value;
            }
            else
            {
                return 0;
            }
        }

        public void SetInstanceValue(mainStat key, float value)
        {
            if (statData.instanceValue.ContainsKey(key))
            {
                statData.instanceValue[key] = value;
            }
        }

        public float GetDamageReduction()
        {
            return statData.damageReduction;
        }

        public override ISaveable Save()
        {
            return statData;
        }

        public override void Load(object data)
        {
            statData = ((JObject)data).ToObject<PlayerStats>();
        }
        public override void SetDefaultValue()
        {
            statData.CopyFrom(_defaultStatData);
        }

        public void SetValue(mainStat key, float value)
        {
            if (statData.defaultValue.ContainsKey(key))
            {
                statData.defaultValue[key] = value;
            }
        }

         public void UpgradeWeapon(float increaseAmount, bool isPercent)
        {
            StatsModifier weaponUpgrade = new StatsModifier();
            weaponUpgrade.isPercent = isPercent;
            weaponUpgrade.statsToModify = new SerializedDictionary<mainStat, float>();
            weaponUpgrade.statsToModify[mainStat.AttackValue] = increaseAmount;

            modifiers.Add(weaponUpgrade);
        }

         public void UpgradeShield(float increaseAmount, bool isPercent, float damageReductionIncrease)
        {
            StatsModifier shieldUpgrade = new StatsModifier();
            shieldUpgrade.isPercent = isPercent;
            shieldUpgrade.statsToModify = new SerializedDictionary<mainStat, float>();
            shieldUpgrade.statsToModify[mainStat.ShieldValue] = increaseAmount;

            modifiers.Add(shieldUpgrade);

            statData.damageReduction += damageReductionIncrease;
        }


    }
    [System.Serializable]
    public class PlayerStats : ISaveable
    {
        public SerializedDictionary<mainStat, float> defaultValue;
        public SerializedDictionary<mainStat, float> instanceValue;
        public int playerExp;
        public float damageReduction;

        public void CopyFrom(ISaveable obj)
        {
            var target = (PlayerStats)obj;
            this.defaultValue = new(target.defaultValue);
            this.instanceValue = new(target.instanceValue);
            this.playerExp = target.playerExp;
            this.damageReduction = target.damageReduction; 
        }
    }

    public enum mainStat
    {
        Health,
        Stamina,
        MaxStamina,
        AttackValue,
        MovementSpeed,
        Defense,
        ShieldValue,
    }
}