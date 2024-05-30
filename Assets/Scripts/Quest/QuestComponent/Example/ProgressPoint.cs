using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CC.Event;
using CC.Inventory;
using CC.Events;
using CC.NPC;

namespace CC.Quest.Example
{
    public class ProgressPoint : MonoBehaviour
    {
        [SerializeField] SenderDataEventChannelSO _onProgress;
        [SerializeField] List<NPCQuestDialogueNodeTransfer> _nextNpcList;
        [SerializeField] SenderDataEventChannelSO SendNPCQuestDialogue;
        [ReadOnly] QuestManager _manager;
        private void Start()
        {
            _manager = FindObjectOfType<QuestManager>();
        }

        public void doProgress()
        {
            _onProgress?.raiseEvent(this, null);
        }
    }
}