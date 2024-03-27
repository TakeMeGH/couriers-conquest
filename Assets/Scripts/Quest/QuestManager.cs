using UnityEngine;
using CC.Core.Data.Dynamic;

namespace CC.Quest
{
    public class QuestManager : MonoBehaviour
    {

        [SerializeField] QuestDatabaseSO _database;
        [SerializeField] PlayerStateSO _playerState;
        [SerializeField] AQuest acceptedQuest;

        public void acceptQuest(Component sender, object data)
        {
            if (acceptedQuest != null)
            {
                Debug.Log("Other quest is on progress");
                return;
            }
            if (data is not int) return;
            int questID = (int)data;
            AQuest temp = _database.GetQuestByID(questID);
            if (temp == null)
            {
                Debug.Log("Quest ID not Found");
                return;
            }
            acceptedQuest = temp;
            acceptedQuest.OnQuestStarted(sender,data);
        }


        public void tryCancelQuest(Component sender, object data)
        {
            if (data is not int) return;
            int questID = (int)data;
            if (questID != acceptedQuest.GetQuestID()) { Debug.Log("you have not accepted this quest"); return; }
            CancelQuest(sender, data);
        }

        #region "Quest Flow"
        public void StartQuest(Component sender, object data)
        {
            acceptedQuest.OnQuestStarted(sender,data);
        }

        public void ProgressQuest(Component sender, object data)
        {
            acceptedQuest.OnQuestProgress(sender,data);
        }

        public void CompleteQuest(Component sender, object data)
        {
            acceptedQuest.OnQuestFinished(sender, data);
            _playerState.addFinishedQuest(acceptedQuest.GetQuestID());
            acceptedQuest = null;
        }

        public void CancelQuest(Component sender, object data)
        {
            acceptedQuest.OnQuestCancelled(sender, data);
            acceptedQuest = null;
        }
        #endregion "Quest Flow"
    }
}
