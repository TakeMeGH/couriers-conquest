using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using CC.Core.Data.Dynamic;

namespace CC.Core.Data.Stable
{
    public class StatsModifierSO : ScriptableObject
    {
        public StatsModifier modifier;
    }
    [System.Serializable]
    public class StatsModifier
    {
        public bool isPercent;
        public SerializedDictionary<mainStat, float> statsToModify;
    }
}
