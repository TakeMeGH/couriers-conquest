using System;
using System.Collections;
using System.Collections.Generic;
using CC.Core.Data.Dynamic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using CC.Inventory.Item;

namespace CC.Inventory
{
    public class PlayerInventoryAction : IInventoryAction
    {
        private InventoryData _inventoryData;
        private PlayerInventoryManager _playerInventoryManager;
        private ItemSlotMouse _itemSlotMouse;

        public void Initialize(InventoryData inventoryData, IInventoryManager inventoryManager, ItemSlotMouse itemSlotMouse)
        {
            _inventoryData = inventoryData;
            _playerInventoryManager = (PlayerInventoryManager)inventoryManager;
            _itemSlotMouse = itemSlotMouse;

        }

        public void OnDropItem()
        {
            if (_itemSlotMouse.itemSlot.item != null && !EventSystem.current.IsPointerOverGameObject())
            {
                AttempToDrop(_itemSlotMouse.itemSlot.item);
            }
        }

        public void AttempToDrop(ABaseItem item)
        {
            if (item == null)
            {
                Debug.Log("Could not find Item in Dictionary to add to drop");
                return;
            }

            if (item.GetItemType() == ItemType.QuestItem)
            {
                RemoveQuestItemEvent((QuestItem)item);

            }
            Transform camTransform = UnityEngine.Camera.main.transform;
            Transform _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            GameObject dropPrefab = ObjectPooling.SharedInstance.GetPooledObject(PoolObjectType.Item);
            if (dropPrefab != null)
            {
                dropPrefab.transform.position = _playerTransform.position + new Vector3(0, 1.8f, 0) + camTransform.forward;
                dropPrefab.transform.rotation = _playerTransform.rotation;
                dropPrefab.SetActive(true);
            }

            Rigidbody rb = dropPrefab.GetComponent<Rigidbody>();
            if (rb != null) rb.velocity = camTransform.forward * _inventoryData.dropSpeed;

            ItemPickup ip = dropPrefab.GetComponentInChildren<ItemPickup>();
            ip.isDropItem = true;
            if (ip != null)
            {
                ip.item = item;
                ip.amount = _itemSlotMouse.splitSize;
                _itemSlotMouse.itemSlot.stacks -= _itemSlotMouse.splitSize;
            }

            if (_itemSlotMouse.itemSlot.stacks < 1) _playerInventoryManager.ClearSlot(_itemSlotMouse.itemSlot);
            _itemSlotMouse.EmptySlot();
            _playerInventoryManager.RefreshInventory();
        }

        public bool CheckItem(ABaseItem _item)
        {
            foreach (ItemSlotInfo i in _inventoryData.items)
            {
                if (_item == i.item)
                {
                    return true;
                }
            }
            return false;
        }

        public void UpgradeItem(ABaseItem _item)
        {
            foreach (ItemSlotInfo i in _inventoryData.items)
            {
                if (_item == i.item)
                {
                    EquipmentItem equipment = (EquipmentItem)i.item;

                    foreach (mainStat stat in Enum.GetValues(typeof(mainStat)))
                    {
                        float additionValue = 0;
                        if (equipment.EquipmentStats.TryGetValue(stat, out float value) &&
                            equipment.upgradeRequiriment[equipment.equipmentLevel].EquipmentStats.TryGetValue(stat, out additionValue)) { }
                        {
                            equipment.EquipmentStats[stat] = value + additionValue;
                        }
                    }
                    equipment.equipmentLevel++;
                    return;
                }
            }
        }
        public void RemoveQuestItemEvent(QuestItem item)
        {
            item.ReduceItemQualityOnDrop();
        }

    }
}
