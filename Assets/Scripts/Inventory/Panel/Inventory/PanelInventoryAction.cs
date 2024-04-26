using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CC.Inventory
{
    public class PanelInventoryAction : IPanelAction
    {
        private AItemPanel _inventoryPanel;
        private IInventoryManager _inventory;
        private ItemSlotMouse _mousePanel;
        private ItemSlotInfo _itemSlot;
        private Image _itemImage;
        private ItemSlotType _slotType;

        public void Initialize(AItemPanel inventoryPanel, IInventoryManager inventory, ItemSlotMouse mousePanel, ItemSlotInfo itemSlot, Image itemImage, ItemSlotType slotType)
        {
            _inventoryPanel = inventoryPanel;
            _inventory = inventory;
            _mousePanel = mousePanel;
            _itemSlot = itemSlot;
            _itemImage = itemImage;
            _slotType = slotType;
        }

        public void OnAction()
        {
            if (_inventory != null)
            {

                //Grab item if mouse slot is empty
                if (_mousePanel.itemSlot.item == null)
                {
                    if (_itemSlot.item != null)
                    {
                        PickupItem();
                        FadeOut();
                    }
                }

                else
                {
                    if (_slotType == ItemSlotType.Inventory)
                    {
                        OnActionInventory();
                    }
                    else if (_slotType == ItemSlotType.Consumable)
                    {
                        OnActionConsumableItem();
                    }else if(_slotType == ItemSlotType.Rune)
                    {
                        OnActionRuneItem();
                    }
                    else
                    {
                        OnActionEquipmentSlot();
                    }
                }
            }
        }

        private void PickupItem()
        {
            _mousePanel.itemSlot = _itemSlot;
            _mousePanel.sourceItemPanel = _inventoryPanel;
            if (Input.GetKey(KeyCode.LeftShift) && _itemSlot.stacks > 1) _mousePanel.splitSize = _itemSlot.stacks / 2;
            else _mousePanel.splitSize = _itemSlot.stacks;
            _mousePanel.SetUI();
        }

        private void FadeOut()
        {
            _itemImage.CrossFadeAlpha(0.3f, 0.05f, true);
        }

        private void DropItem()
        {
            _itemSlot.item = _mousePanel.itemSlot.item;
            if (_mousePanel.splitSize < _mousePanel.itemSlot.stacks)
            {
                _itemSlot.stacks = _mousePanel.splitSize;
                _mousePanel.itemSlot.stacks -= _mousePanel.splitSize;
                _mousePanel.EmptySlot();
            }
            else
            {
                _itemSlot.stacks = _mousePanel.itemSlot.stacks;
                _inventory.ClearSlot(_mousePanel.itemSlot);
            }
        }
        private void SwapItem(ItemSlotInfo slotA, ItemSlotInfo slotB)
        {
            ItemSlotInfo tempItem = new ItemSlotInfo(slotA.item, slotA.stacks);

            slotA.item = slotB.item;
            slotA.stacks = slotB.stacks;

            slotB.item = tempItem.item;
            slotB.stacks = tempItem.stacks;
        }
        private void StackItem(ItemSlotInfo source, ItemSlotInfo destination, int amount)
        {
            int slotsAvailable = destination.item.maxStacks - destination.stacks;
            if (slotsAvailable == 0) return;

            if (amount > slotsAvailable)
            {
                source.stacks -= slotsAvailable;
                destination.stacks = destination.item.maxStacks;
            }
            if (amount <= slotsAvailable)
            {
                destination.stacks += amount;
                if (source.stacks == amount) _inventory.ClearSlot(source);
                else source.stacks -= amount;
            }
        }

        private void OnActionInventory()
        {
            //Clicked on original slot
            if (_itemSlot == _mousePanel.itemSlot)
            {
                _mousePanel.EmptySlot();
            }
            //Clicked on empty slot
            else if (_itemSlot.item == null)
            {
                DropItem();
            }
            else if (_itemSlot.item != _mousePanel.itemSlot.item)
            {
                SwapItem(_itemSlot, _mousePanel.itemSlot);
            }
            //Clicked on occupied slot of different item type
            else if (_itemSlot.item.maxStacks != _mousePanel.itemSlot.item.maxStacks)
            {
                SwapItem(_itemSlot, _mousePanel.itemSlot);
            }
            //Clicked on occupided slot of same type
            else if (_itemSlot.stacks < _itemSlot.item.maxStacks)
            {
                StackItem(_mousePanel.itemSlot, _itemSlot, _mousePanel.splitSize);
            }
            else if (_itemSlot.stacks >= _itemSlot.item.maxStacks)
            {
                SwapItem(_itemSlot, _mousePanel.itemSlot);
            }
            else
            {
                _mousePanel.EmptySlot();
            }

            _inventory.RefreshInventory();
        }

        private void OnActionEquipmentSlot()
        {
            if (_mousePanel.itemSlot.item.GetItemType() != ItemType.Equipment)
            {
                Debug.Log("Slot Not Match");
                return;
            };

            EquipmentItem equipmentScript = (EquipmentItem)_mousePanel.itemSlot.item;

            if (_slotType == equipmentScript.specificType)
            {
                OnActionInventory();
            }
            else
            {
                Debug.Log("Slot Not Match");
            }
        }

        private void OnActionConsumableItem()
        {
            if (_mousePanel.itemSlot.item.GetItemType() == ItemType.Consumable)
            {
                OnActionInventory();
            }
            else
            {
                Debug.Log("Slot Not Match");
            }
        }

        private void OnActionRuneItem()
        {
            if (_mousePanel.itemSlot.item.GetItemType() == ItemType.Rune)
            {
                OnActionInventory();
            }
            else
            {
                Debug.Log("Slot Not Match");
            }
        }

        public void RefreshInventory()
        {
            _inventory.RefreshInventory();
        }
    }
}
