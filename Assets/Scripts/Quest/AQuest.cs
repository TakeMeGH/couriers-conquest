using CC.Event;
using CC.Inventory;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using CC.Events;

namespace CC.Quest
{
    public abstract class AQuest : ScriptableObject
    {
        [SerializeField] int _questID;
        [SerializeField] QuestType _questType;
        [SerializeField] string _questName;
        [TextArea][SerializeField] string _questDescription;
        [SerializeField] string _questArea;
        [SerializeField] int[] _questPrerequisitesID;
        [Header("Reward Event")]
        [SerializeField] ItemInventoryEventChannel _addItemToInventory;
        [SerializeField] OnUpdateCurrencyEventChannel _onUpdateCurrency;
        [SerializeField] IntEventChannelSO _onUpdateExp;

        [SerializeField] SenderDataEventChannelSO _onSpawnBanner;

        [SerializeField] Reward _reward;

        public virtual void OnQuestStarted(Component sender, object data) { }
        public virtual void OnQuestProgress(Component sender, object data) { }
        public virtual void OnQuestFinished(Component sender, object data)
        {
            _onSpawnBanner?.raiseEvent(null, _questName);
        }
        public virtual void OnQuestCancelled(Component sender, object data) { }

        public void SendReward(Component sender, object data)
        {
            foreach (ABaseItem item in _reward.item.Keys)
            {
                _addItemToInventory?.RaiseEvent(item, _reward.item[item]);
            }

            if (_reward.gold > 0)
            {
                _onUpdateCurrency?.RaiseEvent(_reward.gold);
            }

            if (_reward.exp > 0)
            {
                _onUpdateExp?.RaiseEvent(_reward.exp);
            }
        }

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
