using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using CC.Core.Data.Dynamic;

namespace CC.Core.Data.Stable
{
    public class StatsModifierSO : ScriptableObject
    {
        public bool isPercent;
        public SerializedDictionary<mainStat, float> statsToModify;
    }
}
