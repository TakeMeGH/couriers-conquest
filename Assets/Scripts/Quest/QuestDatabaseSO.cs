using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using AYellowpaper.SerializedCollections;
using CC.Core.Data.Dynamic;

namespace CC.Quest
{
    [CreateAssetMenu(menuName = "Database/QuestDataBase")]
    public class QuestDatabaseSO : ScriptableObject
    {
        [SerializeField] SerializedDictionary<int, AQuest> _questDict;
        [SerializeField] PlayerStateSO _playerState;

        public AQuest GetQuestByID(int id)
        {
            if (_questDict.TryGetValue(id, out var result))
            {
                return Instantiate(result);
            }
            return null;
        }

        public List<AQuest> getUnlockedQuest()
        {
            List<AQuest> res = new();
            foreach (var key in _questDict.Keys)
            {
                if (checkPrequisites(key)) res.Add(_questDict[key]);
            }
            return res;
        }

        public bool checkPrequisites(int key)
        {
            if (_questDict[key].GetPrerequisites().Length == 0) { return true; }
            foreach (var pre in _questDict[key].GetPrerequisites())
            {
                if (!isQuestUnlocked(pre)) return false;
            }
            return true;
        }

        public bool isQuestUnlocked(int id)
        {
            if (_playerState.GetFinishedQuest().Length == 0 || !_playerState.GetFinishedQuest().Contains(id)) return false;
            return true;
        }
    }
}
