using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using CC.Core.Save;

namespace CC.Core.Data.Dynamic
{
    [CreateAssetMenu(menuName = "Data/Dynamics/PlayerStates")]
    public class PlayerStateSO : ASavableModel
    {
        [SerializeField] PlayerStateData _state;

        public override ISaveable Save()
        {
            return _state;
        }
        public override void Load(object data)
        {
            _state = ((JObject)data).ToObject<PlayerStateData>();
        }
        public void addFinishedQuest(int id)
        {
            if (_state.finishedQuest.Contains(id)) return;
            _state.finishedQuest.Add(id);
        }
        #region "Getter"
        public int[] GetFinishedQuest()
        {
            return _state.finishedQuest.ToArray();
        }
        #endregion "Getter"
    }

    [System.Serializable]
    public class PlayerStateData : ISaveable
    {
        public Vector3 currentPosition;
        public List<int> finishedQuest;
    }
}
