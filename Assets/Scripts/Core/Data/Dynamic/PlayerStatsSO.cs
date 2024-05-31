using CC.Core.Save;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using Newtonsoft.Json.Linq;
using CC.Core.Data.Stable;
using UnityEngine.InputSystem;
using System;
using UnityEngine.Events;

namespace CC.Core.Data.Dynamic
{
    [CreateAssetMenu(menuName = "Data/Dynamics/PlayerStats")]
    public class PlayerStatsSO : ASavableModel
    {
        [SerializeField] PlayerStats statData;
        [SerializeField] List<StatsModifier> modifiers;
        [Header("default")]
        [SerializeField] PlayerStats _defaultStatData;
        [SerializeField] public UnityAction OnStatChange;
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
                statData.instanceValue[key] = Math.Min(GetValue(key), value);
                OnStatChange.Invoke();
            }
        }

        public StatsModifier AddModifier(mainStat key, float value)
        {
            StatsModifier _newModifier = new()
            {
                isPercent = false,
                statsToModify = new SerializedDictionary<mainStat, float>
            {
                { key, value }
            }
            };
            modifiers.Add(_newModifier);
            return _newModifier;
        }

        public void ForceAddModifier(StatsModifier modifier)
        {
            modifiers.Add(modifier);
        }

        public void RemoveModifier(StatsModifier modifier)
        {
            modifiers.Remove(modifier);
        }

        public void ClearAllModifier()
        {
            modifiers.Clear();
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
        AttackValue,
        MovementSpeed,
        Defense,
        ShieldValue,
        ExpMultiplier
    }
}