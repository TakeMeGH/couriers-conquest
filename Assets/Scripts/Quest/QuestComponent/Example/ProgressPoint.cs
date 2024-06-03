using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CC.Event;
using CC.Inventory;
using CC.Events;
using CC.NPC;
using CC.Quest.UI;
using Unity.VisualScripting;

namespace CC.Quest.Example
{
    public class ProgressPoint : MonoBehaviour
    {
        [SerializeField] SenderDataEventChannelSO[] _onProgress;
        [SerializeField] List<NPCQuestDialogueNodeTransfer> _nextNpcList;
        [SerializeField] SenderDataEventChannelSO SendNPCQuestDialogue;
        [SerializeField] SenderDataEventChannelSO _findQuestHintOwner;
        [SerializeField] QuestHintData _questHintData;

        [ReadOnly] QuestManager _manager;
        private void Start()
        {
            _manager = FindObjectOfType<QuestManager>();
        }

        public void doProgress()
        {
            foreach (var eventOnDrop in _onProgress)
            {
                eventOnDrop?.raiseEvent(this, null);
            }

            _findQuestHintOwner?.raiseEvent(this, _questHintData);

        }
    }
}
