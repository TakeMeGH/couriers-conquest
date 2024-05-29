using CC.Event;
using CC.Inventory;
using UnityEngine;
using AYellowpaper.SerializedCollections;

namespace CC.Quest
{
    public abstract class AQuest : ScriptableObject
    {
        [SerializeField] int _questID;
        [SerializeField] QuestType _questType;
        [SerializeField] string _questName;
        [TextArea] [SerializeField] string _questDescription;
        [SerializeField] string _questArea;
        [SerializeField] int[] _questPrerequisitesID;
        [SerializeField] SenderDataEventChannelSO _onSendReward;
        [SerializeField] Reward _reward;

        public virtual void OnQuestStarted(Component sender, object data) { }
        public virtual void OnQuestProgress(Component sender, object data) { }
        public virtual void OnQuestFinished(Component sender, object data) { _onSendReward?.raiseEvent(null, _reward); }
        public virtual void OnQuestCancelled(Component sender, object data) { }

        #region "Getter"
        public string GetQuestName()
        {
            return _questName;
        }

        public string GetQuestDescription()
        {
            return _questDescription;
        }

        public int GetQuestID()
        {
            return _questID;
        }

        public QuestType GetQuestType()
        {
            return _questType;
        }

        public int[] GetPrerequisites()
        {
            return _questPrerequisitesID;
        }

        public string GetQuestArea()
        {
            return _questArea;
        }

        public Reward GetReward()
        {
            return _reward;
        }
        #endregion "Getter"
    }

    public enum QuestType
    {
        Tutorial,
        Urgent,
        Side
    }

    [System.Serializable]
    public class Reward
    {
        public int gold;
        public int exp;
        public SerializedDictionary<ABaseItem, int> item;
    }
}
