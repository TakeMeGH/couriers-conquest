using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CC.Event;
using CC.Inventory;
using CC.Events;

namespace CC.Quest.Example
{
    public class ExDropPoint : MonoBehaviour
    {
        [SerializeField] SenderDataEventChannelSO _onDrop;
        [SerializeField] ItemInventoryCheckEventChannel _itemCheckEvent;
        [SerializeField] SenderDataEventChannelSO _questItemRemoveEvent;
        [ReadOnly] QuestManager _manager;

        [ReadOnly] ABaseItem _itemToDeliver;

        private void Start()
        {
            _manager = FindObjectOfType<QuestManager>();
        }

        public void Init(ABaseItem _itemToDeliver)
        {
            this._itemToDeliver = _itemToDeliver;
        }
        public void doDrop()
        {
            if (_itemCheckEvent.RaiseEvent(_itemToDeliver))
            {
                _onDrop?.raiseEvent(this, null);
                _questItemRemoveEvent?.raiseEvent(this, _itemToDeliver);
            }
        }
    }
}
