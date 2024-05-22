using CC.Events;
using CC.Interaction;
using CC.Inventory;
using CC.Inventory.Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

namespace CC.Items
{
    public class ChestTreasure : MonoBehaviour
    {
        [SerializeField] private ChestData _items;
        private CustomInterractables _customInteractables;
        [SerializeField] private string _chestName = "Chest";
        public ChestData items { get => _items; set => items = value; }

        public void Initialize()
        {
            if (_items.isColectable)
            {
                _customInteractables = new CustomInterractables();
                OnItemSet();
            }
            else
            {
                transform.parent.gameObject.SetActive(false);
            }
        }

        public void Interact()
        {
            foreach(ChestValue item in _items.itemChest)
            {
                AttempToDrop(item);
            }

            gameObject.SetActive(false);
            transform.parent.gameObject.SetActive(false);
        }

        public void OnItemSet()
        {
            if (_customInteractables == null) _customInteractables = GetComponent<CustomInterractables>();

            _customInteractables.SetName(_chestName);
            //_customInteractables.SetIcon(_item.itemSprite);
        }

        public void AttempToDrop(ChestValue item)
        {
            if (item == null)
            {
                Debug.Log("Could not find Item in Dictionary to add to drop");
                return;
            }

            Transform camTransform = UnityEngine.Camera.main.transform;
            Transform _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            GameObject dropPrefab = ObjectPooling.SharedInstance.GetPooledObject(PoolObjectType.Item);


            //Dropped The Prefab
            if (dropPrefab != null)
            {
                dropPrefab.transform.position = _playerTransform.position + new Vector3(0, 1.8f, 0) + camTransform.forward;
                dropPrefab.transform.rotation = _playerTransform.rotation;
                dropPrefab.SetActive(true);
            }

            Rigidbody rb = dropPrefab.GetComponent<Rigidbody>();
            if (rb != null) rb.velocity = camTransform.forward * 1;

            //Proses Add Item To Inventory
            ItemPickup ip = dropPrefab.GetComponentInChildren<ItemPickup>();
            ip.isDropItem = true;
            if (ip != null)
            {
                ip.item = item.item;
                ip.amount = item.amount;
                ip.Interact();
            }
        }
    }
}
