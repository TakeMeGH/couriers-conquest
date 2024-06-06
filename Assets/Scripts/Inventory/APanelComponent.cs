using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CC.Inventory
{
    public abstract class APanelComponent
    {
        protected AItemPanel _inventoryPanel;
        protected IInventoryManager _inventory;
        protected ItemSlotMouse _mousePanel;
        protected ItemSlotInfo _itemSlot;
        protected Image _itemImage;
        protected ItemSlotType _slotType;

        protected void PickupItem()
        {
            //Debug.Log("Select Item");
            _mousePanel.itemSlot = _itemSlot;
            _mousePanel.sourceItemPanel = _inventoryPanel;
            //SetSlotType();
            _mousePanel.SetUI();
        }

        protected void FadeOut()
        {
            _itemImage.CrossFadeAlpha(0.3f, 0.05f, true);
        }

        protected void DropItem()
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

        #region Temp Code

        /*protected void SwapItem(ItemSlotInfo slotA, ItemSlotInfo slotB)
        {
            if (_mousePanel.slotType == _inventoryPanel.GetSlotType())
            {
                ItemSlotInfo tempItem = new ItemSlotInfo(slotA.item, slotA.stacks);

                slotA.item = slotB.item;
                slotA.stacks = slotB.stacks;

                slotB.item = tempItem.item;
                slotB.stacks = tempItem.stacks;
            }
            else if (CheckInventoryItemType(_inventoryPanel.itemSlot.item))
            {
                ItemSlotInfo tempItem = new ItemSlotInfo(slotA.item, slotA.stacks);

                slotA.item = slotB.item;
                slotA.stacks = slotB.stacks;

                slotB.item = tempItem.item;
                slotB.stacks = tempItem.stacks;
            }
            else
            {
                _mousePanel.EmptySlot();
                _inventory.RefreshInventory();
            }
        }


        protected void StackItem(ItemSlotInfo source, ItemSlotInfo destination, int amount)
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

        protected void OnActionInventory()
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

        protected void OnActionEquipmentSlot()
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

        protected void OnActionConsumableItem()
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

        protected void OnActionRuneItem()
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

        private void SetSlotType()
        {
            if (_inventoryPanel.GetSlotType() != ItemSlotType.Inventory)
            {
                ItemType type = _inventoryPanel.itemSlot.item.GetItemType();
                if (type == ItemType.Consumable)
                {
                    _mousePanel.slotType = ItemSlotType.Consumable;
                }
                else if (type == ItemType.Equipment)
                {
                    _mousePanel.slotType = ItemSlotType.Equipment;
                }
                else
                {
                    _mousePanel.slotType = ItemSlotType.Inventory;
                }
            }
            else
            {
                _mousePanel.slotType = ItemSlotType.Inventory;
            }
        }

        private bool CheckInventoryItemType(ABaseItem item)
        {
            ItemType type = item.GetItemType();
            if (type == ItemType.Consumable && (_inventoryPanel.GetSlotType() == ItemSlotType.Consumable || _mousePanel.slotType == ItemSlotType.Consumable))
            {
                return true;
            }
            else if (type == ItemType.Rune && (_inventoryPanel.GetSlotType() == ItemSlotType.Rune || _mousePanel.slotType == ItemSlotType.Rune))
            {
                return true;
            }
            else if (type == ItemType.Equipment && (_inventoryPanel.GetSlotType() == ItemSlotType.Equipment || _mousePanel.slotType == ItemSlotType.Equipment))
            {
                return true;
            }
            else
            {
                return false;
            }

        }*/

        #endregion
    }
}
