using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Inventory
{
    public class PlayerInventoryManagement : IInventoryManagement
    {
        private InventoryData _inventoryData;
        private PlayerInventoryManager _playerInventoryManager;
        private ItemSlotMouse _itemSlotMouse;
        private ItemDictionary _itemDictionary;

        public void Initialize(InventoryData inventoryData, IInventoryManager inventoryManager, ItemSlotMouse itemSlotMouse)
        {
            _inventoryData = inventoryData;
            _playerInventoryManager = (PlayerInventoryManager)inventoryManager;
            _itemSlotMouse = itemSlotMouse;

            _itemDictionary = new ItemDictionary();
            _itemDictionary.Initialize();

            DefaultItem();
            SetDefaultEquipment();
        }

        private void DefaultItem()
        {
            for (int i = 0; i < _playerInventoryManager.itemPanelGrid.Length; i++)
            {
                AItemPanel[] itemPanelsInGrid = _playerInventoryManager.itemPanelGrid[i].GetComponentsInChildren<AItemPanel>();
                _playerInventoryManager.existingPanels.AddRange(itemPanelsInGrid);
            }

            //_inventoryData.items.Clear();
            for (int i = 0; i < _playerInventoryManager.existingPanels.Count; i++)
            {
                //_inventoryData.items.Add(new ItemSlotInfo(null, 0));
                _playerInventoryManager.existingPanels[i].mousePanel = _itemSlotMouse;
            }
        }

        private void SetDefaultEquipment()
        {

            for (int i = _inventoryData.inventorySize; i < _playerInventoryManager.existingPanels.Count; i++)
            {
                AttachDefaultItem(i);
            };
        }

        public int OnAddItem(ABaseItem item, int amount)
        {
            Debug.Log("Add Item");

            if (item == null)
            {
                Debug.Log("Could not find Item in Dictionary to add to Inventory");
                return amount;
            }

            //Check for open spaces in existing slots
            foreach (ItemSlotInfo i in _inventoryData.items)
            {
                if (i.item != null)
                {
                    if (i.item.itemName == item.itemName)
                    {
                        if (amount > i.item.maxStacks - i.stacks)
                        {
                            amount -= i.item.maxStacks - i.stacks;
                            i.stacks = i.item.maxStacks;
                        }
                        else
                        {
                            i.stacks += amount;
                            if (_playerInventoryManager.inventoryMenuUI.activeSelf) _playerInventoryManager.RefreshInventory();
                            return 0;
                        }
                    }
                }
            }
            //Fill empty slots with leftover items
            foreach (ItemSlotInfo i in _inventoryData.items)
            {
                if (i.item == null)
                {
                    if (amount > item.maxStacks)
                    {
                        i.item = item;
                        i.stacks = item.maxStacks;
                        amount -= item.maxStacks;
                    }
                    else
                    {
                        i.item = item;
                        i.stacks = amount;
                        if (_playerInventoryManager.inventoryMenuUI.activeSelf) _playerInventoryManager.RefreshInventory();
                        return 0;
                    }
                }
            }

            if (_playerInventoryManager.inventoryMenuUI.activeSelf) _playerInventoryManager.RefreshInventory();
            return amount;
        }

        private void AttachDefaultItem(int targetSlot)
        {
            for (int i = 0; i < _inventoryData.items.Count; i++)
            {
                if (_inventoryData.items[i].item == null) continue;

                if (_inventoryData.items[i].item.GetItemType() == ItemType.Equipment)
                {
                    if (((EquipmentItem)_inventoryData.items[i].item).specificType == _playerInventoryManager.existingPanels[targetSlot].GetSlotType())
                    {
                        _inventoryData.items[targetSlot].item = _inventoryData.items[i].item;
                        _inventoryData.items[targetSlot].stacks = _inventoryData.items[i].stacks;
                        _inventoryData.items[i].item = null;
                        break;
                    }
                    else
                    {
                        //Debug.Log(targetSlot.ToString() + " Not Added");
                    }
                }
            }

            _playerInventoryManager.RefreshInventory();
        }

        public void OnRemoveItem(Component _component, object _item)
        {
            ABaseItem _itemToRemove = (ABaseItem)_item;
            foreach (ItemSlotInfo i in _inventoryData.items)
            {
                if (_itemToRemove == i.item)
                {
                    i.item = null;
                    break;
                }
            }
        }

        public void OnSellItem(ABaseItem itemSell, int amount)
        {
            int _currentAmmount = amount;
            foreach (ItemSlotInfo i in _inventoryData.items)
            {
                if (i.item == itemSell)
                {
                    if (i.stacks < _currentAmmount)
                    {
                        _currentAmmount -= i.stacks;
                        i.stacks = 0;
                    }
                    else
                    {
                        i.stacks -= _currentAmmount;
                        _currentAmmount = 0;

                    }
                    if (i.stacks <= 0)
                    {
                        i.item = null;
                    }

                    if (_currentAmmount == 0)
                    {
                        break;
                    }
                }
            }
        }

        public void OnUpdateCurrency(float amount)
        {
            _inventoryData.playerGold += amount;
        }
    }
}
