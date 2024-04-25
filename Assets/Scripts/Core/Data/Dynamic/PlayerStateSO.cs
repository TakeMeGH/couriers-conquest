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
        [SerializeField] PlayerStateData _defaultState;
        public override ISaveable Save()
        {
            return _state;
        }
        public override void Load(object data)
        {
            _state = ((JObject)data).ToObject<PlayerStateData>();
        }
        #region "Setter"
        public void SaveCurrentPosition(Vector3 pos)
        {
            _state.currentPosition = pos;
        }
        public void addFinishedQuest(int id)
        {
            if (_state.finishedQuest.Contains(id)) return;
            _state.finishedQuest.Add(id);
        }
        #endregion "Setter"
        #region "Getter"
        public int[] GetFinishedQuest()
        {
            return _state.finishedQuest.ToArray();
        }
        public Vector3 GetSavedPosition()
        {
            return _state.currentPosition;
        }
        #endregion "Getter"

        public override void SetDefaultValue()
        {
            _state.CopyFrom(_defaultState);
        }
    }

    [System.Serializable]
    public class PlayerStateData : ISaveable
    {
        public Vector3 currentPosition;
        public List<int> finishedQuest;
        public void CopyFrom(ISaveable obj)
        {
            var target = (PlayerStateData)obj;
            this.currentPosition = target.currentPosition;
            this.finishedQuest = new(target.finishedQuest);
        }
    }
}
