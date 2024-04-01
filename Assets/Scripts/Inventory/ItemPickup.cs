using System.Collections;
using UnityEngine;

namespace cc_inventory
{
    public class ItemPickup : MonoBehaviour
    {
        [SerializeField] private ABaseItem _item;
        [SerializeField] private int _amount = 1;
        private bool isPickup = false;

        public bool isDropItem = false;

        public ABaseItem item { set => _item = value; }
        public int amount { set => _amount = value; }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                if(!isPickup)
                {
                    InventoryManager playerInventory = other.GetComponent<InventoryManager>();

                    if (playerInventory != null) PickUpItem(playerInventory);
                }
            }
        }

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

        public void PickUpItem(InventoryManager inventory)
        {
            _amount = inventory.AddItem(_item, _amount);

            if (isDropItem)
            {
                if (_amount < 1) transform.parent.gameObject.SetActive(false); ;
            }
            else
            {
                if (_amount < 1) Destroy(this.transform.root.gameObject);
            }
        }

    }
}