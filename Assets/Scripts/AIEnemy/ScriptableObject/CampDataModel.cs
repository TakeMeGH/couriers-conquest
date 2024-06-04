using System.Collections.Generic;
using CC.Core.Save;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CC
{
    [CreateAssetMenu(menuName = "Data/Dynamics/CampDataModel")]
    public class CampDataModel : ASavableModel
    {
        public CampData CurrentCampData;
        public CampData DefaultCampData;
        public int DayCountToSpawn;
        public override ISaveable Save()
        {
            return CurrentCampData;
        }

        public override void Load(object data)
        {
            CurrentCampData = ((JObject)data).ToObject<CampData>();
        }

        public override void SetDefaultValue()
        {
            CurrentCampData.CopyFrom(DefaultCampData);
        }

        public void ResetEnemyAlive()
        {
            for (int i = 0; i < CurrentCampData.IsAlive.Count; i++)
            {
                CurrentCampData.IsAlive[i] = true;
            }
        }

    }

    [System.Serializable]
    public class CampData : ISaveable
    {
        public List<bool> IsAlive;
        public int CurrentDay;
        public void CopyFrom(ISaveable obj)
        {
            var target = (CampData)obj;
            this.IsAlive = target.IsAlive;
            this.CurrentDay = target.CurrentDay;
        }
    }

}
