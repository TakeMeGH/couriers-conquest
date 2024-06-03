using CC.Core.Data.Dynamic;
using System.Collections;
using System.Collections.Generic;
using CC.UI.Notification;
using UnityEngine;
using UnityEngine.EventSystems;

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

            DefaultItem(inventoryManager);
            SetDefaultEquipment();
            AddAllQuestItemEvent();

            //CheckDefaultEquipment();
        }

        private void DefaultItem(IInventoryManager inventoryManager)
        {
            for (int i = 0; i < _playerInventoryManager.itemPanelGrid.Length; i++)
            {
                PanelInventory[] itemPanelsInGrid = _playerInventoryManager.itemPanelGrid[i].GetComponentsInChildren<PanelInventory>();
                for (int j = 0; j < itemPanelsInGrid.Length; j++)
                {
                    itemPanelsInGrid[j].Initialize(inventoryManager);
                    PanelInventory script = (PanelInventory)itemPanelsInGrid[j];

                    script.itemIndex = j;
                }
                _playerInventoryManager.existingPanels.AddRange(itemPanelsInGrid);
            }

            //_inventoryData.items.Clear();
            for (int i = 0; i < _playerInventoryManager.existingPanels.Count; i++)
            {
                _playerInventoryManager.existingPanels[i].mousePanel = _itemSlotMouse;
            }

            /*for(int i = _inventoryData.items.Count; i < _playerInventoryManager.existingPanels.Count; i++)
            {
                _inventoryData.items.Add(new ItemSlotInfo(null, 0));
            }*/
        }

        private void SetDefaultEquipment()
        {
            _playerInventoryManager.ClearAllModifier();
            for (int i = _inventoryData.inventoryIndex; i < _playerInventoryManager.existingPanels.Count; i++)
            {
                AttachDefaultItem(i);
                _playerInventoryManager.existingPanels[i].itemIndex = i;
            };
        }

        public int OnAddItem(ABaseItem item, int amount)
        {

            if (item == null)
            {
                Debug.Log("Could not find Item in Dictionary to add to Inventory");
                return amount;
            }

            //Check for open spaces in existing slots

            // TODO : ITEMPICKEDUP
            int totalAmount = amount;


            for (int i = 0; i < _inventoryData.inventorySize; i++)
            {
                if (_inventoryData.items[i].item != null)
                {
                    if (_inventoryData.items[i].item.itemName == item.itemName)
                    {
                        if (amount > _inventoryData.items[i].item.maxStacks - _inventoryData.items[i].stacks)
                        {
                            amount -= _inventoryData.items[i].item.maxStacks - _inventoryData.items[i].stacks;
                            _inventoryData.items[i].stacks = _inventoryData.items[i].item.maxStacks;
                        }
                        else
                        {
                            _inventoryData.items[i].stacks += amount;
                            if (_playerInventoryManager.inventoryMenuUI.activeSelf) _playerInventoryManager.RefreshInventory();
                            _inventoryData.itemPickupUI.raiseEvent(null, new itemNotifData(item.itemName, amount, item.itemSprite));
                            return 0;
                        }
                    }
                }
            }


            for (int i = 0; i < _inventoryData.inventorySize; i++)
            {
                if (_inventoryData.items[i].item == null)
                {
                    if (amount > item.maxStacks)
                    {
                        _inventoryData.items[i].item = item;
                        _inventoryData.items[i].stacks = item.maxStacks;
                        amount -= item.maxStacks;
                    }
                    else
                    {
                        _inventoryData.items[i].item = item;
                        _inventoryData.items[i].stacks = amount;
                        if (_playerInventoryManager.inventoryMenuUI.activeSelf) _playerInventoryManager.RefreshInventory();
                        _inventoryData.itemPickupUI.raiseEvent(null, new itemNotifData(item.itemName, amount, item.itemSprite));
                        return 0;
                    }
                }
            }

            _inventoryData.itemPickupUI.raiseEvent(null, new itemNotifData(item.itemName, totalAmount - amount, item.itemSprite));

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
                    EquipmentItem item = (EquipmentItem)_inventoryData.items[i].item;
                    Debug.Log("Try To Attach");
                    _playerInventoryManager.AddModifier(item, false);

                    if (item.specificType == _playerInventoryManager.existingPanels[targetSlot].GetSlotType())
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

        

        /*private void CheckDefaultEquipment()
        {
            for (int i = _inventoryData.inventoryIndex; i < _playerInventoryManager.existingPanels.Count; i++)
            {
                if (_inventoryData.items[i].item == null)
                {
                    Debug.Log("Null");
                    _inventoryData.items[i].item = _playerInventoryManager._defaultEquipment[i - _inventoryData.inventoryIndex];
                    AttachDefaultItem(i);
                    _playerInventoryManager.existingPanels[i].itemIndex = i;
                }
                else
                {
                    Debug.Log("Not Null");
                }
            };
        }*/

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

        public void OnUpdateCurrency(int amount)
        {
            _inventoryData.itemPickupUI.raiseEvent(null, new itemNotifData("Gold", amount, _inventoryData.GoldSprite));
            _inventoryData.playerGold += amount;
        }

        public void AddAllQuestItemEvent()
        {
            for (int i = 0; i < _inventoryData.items.Count; i++)
            {
                if (_inventoryData.items[i].item == null) continue;

                AddQuestItemEvent(i);
            }
        }

        public void AddQuestItemEvent(int indexItem)
        {
            if (_inventoryData.items[indexItem].item.GetItemType() == ItemType.QuestItem)
            {
                ((QuestItem)_inventoryData.items[indexItem].item).EnableDamageEvent();
            }
        }

        public void OnEquipRune()
        {
            if (_playerInventoryManager.activeSlot == ItemType.Rune && _playerInventoryManager.activeIndexItemSlot != _inventoryData.indexRuneEquiped)
            {
                SetRuneItem();
            }
            else if (_playerInventoryManager.activeSlot == ItemType.Consumable && _playerInventoryManager.activeIndexItemSlot != _inventoryData.indexPouchEquiped)
            {
                SetConsumableItem();
            }
        }

        public void ResetEquipedPanelUI()
        {
            for (int i = 0; i < _playerInventoryManager.existingPanels.Count; i++)
            {
                _playerInventoryManager.existingPanels[i].ShowEquipedPanel(false);
            }
        }

        public void SetRuneItem()
        {
            ResetEquipedPanelUI();
            _playerInventoryManager.existingPanels[_playerInventoryManager.activeIndexItemSlot].ShowEquipedPanel(true);
        }

        public void SetConsumableItem()
        {
            ResetEquipedPanelUI();
            _playerInventoryManager.existingPanels[_playerInventoryManager.activeIndexItemSlot].ShowEquipedPanel(true);
        }
    }
}
