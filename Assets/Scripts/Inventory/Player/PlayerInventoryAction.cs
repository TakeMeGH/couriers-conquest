using System;
using CC.Core.Data.Dynamic;
using UnityEngine;
using UnityEngine.UI;

namespace CC.Inventory
{
    public class PlayerInventoryAction : IInventoryAction
    {
        private InventoryData _inventoryData;
        private PlayerInventoryManager _playerInventoryManager;
        private ItemSlotMouse _itemSlotMouse;

        private Transform _camTransform = null;
        private Transform _playerTransform = null;
        private float _holdTime = 0;
        private float maxHoldDuration = 0.5f;
        private Slider _sliderDrop;
        private bool _hasDroped = false;

        public void Initialize(InventoryData inventoryData, IInventoryManager inventoryManager, ItemSlotMouse itemSlotMouse, Slider slider)
        {
            _inventoryData = inventoryData;
            _playerInventoryManager = (PlayerInventoryManager)inventoryManager;
            _itemSlotMouse = itemSlotMouse;
            _sliderDrop = slider;
        }

        public void HoldAttemp()
        {
            _holdTime += Time.deltaTime;

            if (_holdTime >= maxHoldDuration)
            {
                DropAllItem();
                _playerInventoryManager.isHoldDrop = false;
            }

            UpdateSliderValue();
        }


        private void UpdateSliderValue()
        {
            float sliderValue = Mathf.Clamp01(_holdTime / maxHoldDuration) * _sliderDrop.maxValue;
            _sliderDrop.value = sliderValue;
        }

        public void DropPerformed()
        {
            if (!_playerInventoryManager.canDrop) return;

            _sliderDrop.value = 0;
            _sliderDrop.gameObject.SetActive(true);
            _playerInventoryManager.isHoldDrop = true;
            _holdTime = 0;
        }

        public void DropCanceled()
        {
            if (!_playerInventoryManager.canDrop) return;

            OnDropItem(_playerInventoryManager.activeIndexItemSlot, 1);
            RefreshDropAttempt();
        }

        private void DropAllItem()
        {
            OnDropItem(_playerInventoryManager.activeIndexItemSlot, _inventoryData.items[_playerInventoryManager.activeIndexItemSlot].stacks);
            RefreshDropAttempt();
        }

        private void RefreshDropAttempt()
        {
            _sliderDrop.gameObject.SetActive(false);
            _playerInventoryManager.isHoldDrop = false;
            _holdTime = 0;

            if (CheckActiveItemStack() <= 0)
            {
                Debug.Log("Checking - " + _playerInventoryManager.activeIndexItemSlot.ToString());
                _playerInventoryManager.activeSlot = ItemType.None;
                _playerInventoryManager.itemDetailPanel.SetActive(false);
                _playerInventoryManager.existingPanels[_playerInventoryManager.activeIndexItemSlot].isNull = true;

                if (_playerInventoryManager.activeIndexItemSlot == _inventoryData.indexPouchEquiped)
                {
                    _playerInventoryManager.UnEquipSlot(ItemType.Consumable);
                }else if(_playerInventoryManager.activeIndexItemSlot == _inventoryData.indexRuneEquiped)
                {
                    _playerInventoryManager.UnEquipSlot(ItemType.Rune);
                }

                _playerInventoryManager.RefreshPanelItem();
            }

            _playerInventoryManager.RefreshInventory();
        }

        public int CheckActiveItemStack()
        {
            if (_playerInventoryManager.activeSlot == ItemType.None)
                return 0;

            return _inventoryData.items[_playerInventoryManager.activeIndexItemSlot].stacks;
        }

        public void OnDropItem(int index, int amount)
        {
            if (_playerTransform == null)
            {
                _camTransform = UnityEngine.Camera.main.transform;
                _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            }

            ABaseItem item = _inventoryData.items[index].item;

            if (item == null)
            {
                Debug.Log("Could not find Item in Dictionary to add to drop");
                return;
            }
            else
            {
                _inventoryData.items[index].stacks -= amount;
            }

            if (item.GetItemType() == ItemType.QuestItem)
            {
                RemoveQuestItemEvent((QuestItem)item);
            }

            GameObject dropPrefab = ObjectPooling.SharedInstance.GetPooledObject(PoolObjectType.Item);
            if (dropPrefab != null)
            {
                dropPrefab.transform.position = _playerTransform.position + new Vector3(0, 3f, 0) + _camTransform.forward;
                dropPrefab.transform.rotation = _playerTransform.rotation;
                dropPrefab.SetActive(true);
            }

            Rigidbody rb = dropPrefab.GetComponent<Rigidbody>();
            if (rb != null) rb.velocity = _camTransform.forward * _inventoryData.dropSpeed;

            ItemPickup ip = dropPrefab.GetComponentInChildren<ItemPickup>();
            ip.isDropItem = true;
            if (ip != null)
            {
                ip.item = item;
                ip.amount = amount;
            }

            if (_itemSlotMouse.itemSlot.stacks < 1) _playerInventoryManager.ClearSlot(_itemSlotMouse.itemSlot);
            _itemSlotMouse.EmptySlot();

            //_playerInventoryManager.RefreshInventory();
            //RefreshDropAttempt();
        }

        public bool CheckItem(ABaseItem _item)
        {
            foreach (ItemSlotInfo i in _inventoryData.items)
            {
                if (_item.idItem == i.item.idItem)
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
                    equipment.equipmentLevel++;

                    _playerInventoryManager.AddModifier(equipment, true);
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
