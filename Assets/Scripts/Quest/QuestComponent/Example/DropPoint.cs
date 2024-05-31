using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CC.Event;
using CC.Inventory;
using CC.Events;
using CC.NPC;
using CC.Quest.UI;

namespace CC.Quest.Example
{
    public class DropPoint : MonoBehaviour
    {
        [SerializeField] SenderDataEventChannelSO[] _onDrop;
        [SerializeField] ItemInventoryCheckEventChannel _itemCheckEvent;
        [SerializeField] SenderDataEventChannelSO _questItemRemoveEvent;
        [SerializeField] List<NPCQuestDialogueNodeTransfer> _nextNpcList;
        [SerializeField] SenderDataEventChannelSO SendNPCQuestDialogue;
        [SerializeField] SenderDataEventChannelSO _findQuestHintOwner;
        [SerializeField] QuestHintData _questHintData;

        [ReadOnly] QuestManager _manager;

        [ReadOnly] ABaseItem _itemToDeliver;

        private void Start()
        {
            _manager = FindObjectOfType<QuestManager>();
        }

        public void Init(ABaseItem _itemToDeliver)
        {
            this._itemToDeliver = _itemToDeliver;
            if (_itemToDeliver == null) return;

        }
        public void doDrop()
        {
            if (_itemToDeliver == null || _itemCheckEvent.RaiseEvent(_itemToDeliver))
            {
                foreach (var eventOnDrop in _onDrop){
                    eventOnDrop?.raiseEvent(this, null);
                }

                foreach (var npc in _nextNpcList) SendNPCQuestDialogue?.raiseEvent(this, npc);

                _findQuestHintOwner?.raiseEvent(this, _questHintData);

                if (_itemToDeliver != null) _questItemRemoveEvent?.raiseEvent(this, _itemToDeliver);
            }
        }
    }
}
