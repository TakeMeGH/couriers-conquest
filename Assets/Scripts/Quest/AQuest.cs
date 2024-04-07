using UnityEngine;

namespace CC.Quest
{
    public abstract class AQuest : ScriptableObject
    {
        [SerializeField] int _questID;
        [SerializeField] QuestType _questType;
        [SerializeField] string _questName;
        [TextArea] [SerializeField] string _questDescription;
        [SerializeField] int[] _questPrerequisitesID;

        public virtual void OnQuestStarted(Component sender, object data) { }
        public virtual void OnQuestProgress(Component sender, object data) { }
        public virtual void OnQuestFinished(Component sender, object data) { }
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
        #endregion "Getter"
    }

    public enum QuestType
    {
        Tutorial,
        Urgent,
        Side
    }
}