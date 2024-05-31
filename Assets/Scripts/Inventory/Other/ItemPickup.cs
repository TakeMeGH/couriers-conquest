using System.Collections;
using CC.Event;
using CC.Events;
using CC.Interaction;
using CC.UI.Notification;
using UnityEngine;

namespace CC.Inventory
{
    public class ItemPickup : MonoBehaviour
    {
        [SerializeField] private ABaseItem _item;
        [SerializeField] private int _amount = 1;
        [SerializeField] ItemInventoryEventChannel _addItemToInventory;
        [SerializeField] VoidEventChannelSO _onItemPickup;
        [SerializeField] SenderDataEventChannelSO _itemPickedUP;
        private bool isPickup = false;

        public bool isDropItem = false;
        [SerializeField] CustomInterractables _customInteractables;

        public ABaseItem item
        {
            set
            {
                _item = value;
                OnItemSet();
            }
        }
        public int amount
        {
            set
            {
                _amount = value;
                OnAmountSet();
            }
        }

        private void OnEnable()
        {
            Countdown();
        }


        private void Start()
        {
            _customInteractables = GetComponent<CustomInterractables>();
            OnItemSet();
            OnAmountSet();
        }

        private IEnumerator Countdown()
        {
            isPickup = true;
            yield return new WaitForSeconds(1);
            isPickup = false;
        }

        public void Interact()
        {
            _amount = _addItemToInventory.RaiseEvent(_item, _amount);
            _onItemPickup.RaiseEvent();
            if (isDropItem)
            {
                if (_amount < 1)
                {
                    transform.parent.gameObject.SetActive(false);
                }
            }
            else
            {
                if (_amount < 1)
                {
                    Destroy(this.transform.parent.gameObject);
                }
            }
        }

        public void OnItemSet()
        {
            if (_customInteractables == null) _customInteractables = GetComponent<CustomInterractables>();

            _customInteractables.SetName(_item.itemName);
            _customInteractables.SetIcon(_item.itemSprite);

        }

        public void OnAmountSet()
        {
            if (_customInteractables == null) _customInteractables = GetComponent<CustomInterractables>();

            _customInteractables.SetAmount(_amount);
        }

    }
}