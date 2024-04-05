using CC.Core.Save;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Core.Daytime {
    [CreateAssetMenu(menuName = "Data/Dynamics/DayTimeData")]
    public class DayTimeDataModel :ASavableModel
    {
        [SerializeField] DayTimeData _daytimeData;
        public override ISaveable Save()
        {
            return _daytimeData;
        }
        public override void Load(object data)
        {
            _daytimeData = ((JObject)data).ToObject<DayTimeData>();
        }

        public void OnTimeUpdate(int time)
        {
            _daytimeData.time = time;
        }
        
        public void OnDayUpdate(int day)
        {
            _daytimeData.day = day;
        }

        public int getDay()
        {
            return _daytimeData.day;
        }
        public int getTime()
        {
            return _daytimeData.time;
        }
    }
    [System.Serializable]
    public class DayTimeData : ISaveable
    {
        public int day;
        public int time;
    }
}
