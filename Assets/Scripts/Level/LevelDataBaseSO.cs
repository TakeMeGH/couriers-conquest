using CC.Core.Data.Stable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Level
{
    [CreateAssetMenu(menuName ="Database/LevelDataBase")]
    public class LevelDataBaseSO : ScriptableObject
    {
        public StatsModifier[] stats;
        public int level;
        public float excessiveExp;

        public StatsModifier getModifierByExp(float exp)
        {
            calculateLevel(exp);
            StatsModifier temp = stats[level];
            if(level > stats.Length - 1) return stats[stats.Length - 1];
            return stats[level];
        }

        public void calculateLevel(float exp)
        {
            level = Mathf.FloorToInt(Mathf.Sqrt(exp) * 0.05f);
            excessiveExp = exp - Mathf.Pow(level/0.05f,2);
        }

        #region "Getter"
        public int getLevel()
        {
            return level;
        }
        public float getExcessiveExp()
        {
            return excessiveExp;
        }
        #endregion "Getter"

    }
}
