using CC.Event;
using CC.Events;
using CC.Inventory;
using UnityEngine;
using CC.NPC;
using System.Collections.Generic;
using CC.Quest.UI;


namespace CC.Quest.Example
{
    public class PickupPoint : MonoBehaviour
    {
        [SerializeField] SenderDataEventChannelSO _onPickup;
        [SerializeField] ItemInventoryEventChannel _addItemToInventory;
        [SerializeField] List<NPCQuestDialogueNodeTransfer> _nextNpcList;
        [SerializeField] SenderDataEventChannelSO SendNPCQuestDialogue;
        [SerializeField] SenderDataEventChannelSO _findQuestHintOwner;
        [SerializeField] QuestHintData _questHintData;
        [ReadOnly] QuestManager _manager;

        [ReadOnly] ABaseItem _itemToDeliver;
        bool _isPickup;

        private void Start()
        {
            _manager = FindObjectOfType<QuestManager>();
        }

        public void Init(ABaseItem _itemToDeliver)
        {
            this._itemToDeliver = _itemToDeliver;
            if (_itemToDeliver == null) return;
            if (_itemToDeliver.GetItemType() == ItemType.QuestItem)
            {
                ((QuestItem)this._itemToDeliver).SetDefaultQuality();
            }

        }
        public void doPickup()
        {
            if (_isPickup) return;

            _isPickup = true;

            _onPickup?.raiseEvent(this, null);

            foreach (var npc in _nextNpcList) SendNPCQuestDialogue?.raiseEvent(this, npc);

            _findQuestHintOwner?.raiseEvent(this, _questHintData);

            if (_itemToDeliver != null) _addItemToInventory.RaiseEvent(_itemToDeliver, 1);
        }
    }
}
