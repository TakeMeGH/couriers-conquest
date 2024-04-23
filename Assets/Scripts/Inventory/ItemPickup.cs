using System.Collections;
using CC.Events;
using CC.Interaction;
using UnityEngine;

namespace CC.Inventory
{
    public class ItemPickup : MonoBehaviour, IInteraction
    {
        [SerializeField] private ABaseItem _item;
        [SerializeField] private int _amount = 1;
        [SerializeField] ItemInventoryEventChannel _addItemToInventory;
        [SerializeField] VoidEventChannelSO _onItemPickup;
        private bool isPickup = false;

        public bool isDropItem = false;

        public ABaseItem item { set => _item = value; }
        public int amount { set => _amount = value; }

        private void OnEnable()
        {
            Countdown();
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
                if (_amount < 1) transform.parent.gameObject.SetActive(false); ;
            }
            else
            {
                if (_amount < 1) Destroy(this.transform.parent.gameObject);
            }
        }

    }
}