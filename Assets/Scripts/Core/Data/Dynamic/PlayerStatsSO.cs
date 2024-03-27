using CC.Core.Save;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using Newtonsoft.Json.Linq;
using CC.Core.Data.Stable;

namespace CC.Core.Data.Dynamic
{
    [CreateAssetMenu(menuName = "Data/PlayerStats")]
    public class PlayerStatsSO : ASavableModel
    {
        [SerializeField] PlayerStats statData;
        [SerializeField] List<StatsModifierSO> modifiers;
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

        public override ISaveable Save()
        {
            return statData;
        }

        public override void Load(object data)
        {
            statData = ((JObject)data).ToObject<PlayerStats>();
        }
    }
    [System.Serializable]
    public class PlayerStats : ISaveable
    {
        public SerializedDictionary<mainStat, float> defaultValue;
        public SerializedDictionary<mainStat, float> instanceValue;
        public int playerExp;
    }

    public enum mainStat
    {
        Health,
        Stamina,
        AttackValue,
        MovementSpeed,
        Defense,
    }
}
