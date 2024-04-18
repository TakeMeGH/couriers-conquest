using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using CC.Core.Data.Dynamic;
using CC.Event;
using System.Linq;

namespace CC.Quest.UI
{
    public class QuestViewUI : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] InputReader _inputReader;
        [Header("Models")]
        [SerializeField] QuestDatabaseSO _database;
        [SerializeField] PlayerStateSO _playerState;
        [Header("Quests and object")]
        [SerializeField] List<AQuest> _questList;
        [SerializeField] GameObject _questContainer;
        [SerializeField] AQuest selectedQuest;
        [Header("Objects")]
        [SerializeField] Transform _questContainterLayout;
        [SerializeField] QuestViewDescription _questViewDescription;
        [Header("Event")]
        [SerializeField] SenderDataEventChannelSO _startEvent;
        [SerializeField] SenderDataEventChannelSO _cancelEvent;

        private void Start()
        {
            RefreshList();
        }

        public void RefreshList()
        {
            selectedQuest = null;
            foreach (Transform child in _questContainterLayout)
            {
                Destroy(child.gameObject);
            }
            _questList = new(_database.getUnlockedQuest());
            foreach (var quest in _questList.Select((value, index) => new { index, value }))
            {
                GameObject temp = Instantiate(_questContainer, _questContainterLayout);
                temp.GetComponent<QuestViewContainer>().assign(quest.value, quest.index);
            }
        }

        public void populateDescription(Component sender, object data)
        {
            int ind = (int)data;
            _questViewDescription.assign(_questList[ind]);
            selectedQuest = _questList[ind];
        }

        public void activateQuest()
        {
            if (selectedQuest == null) { Debug.Log("no quest selected"); return; }
            _startEvent?.raiseEvent(this, selectedQuest.GetQuestID());
        }

        public void cancelQuest()
        {
            if (selectedQuest == null) { Debug.Log("no quest selected"); return; }
            _cancelEvent?.raiseEvent(this, selectedQuest.GetQuestID());
        }

        public void openView()
        {
            RefreshList();
            Cursor.lockState = CursorLockMode.Confined;

        }

        public void closeView()
        {
            Cursor.lockState = CursorLockMode.Locked;
            _inputReader.EnableGameplayInput();
        }
    }
}
