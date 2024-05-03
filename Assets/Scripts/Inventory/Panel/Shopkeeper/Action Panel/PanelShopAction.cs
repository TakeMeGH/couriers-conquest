using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CC.Inventory
{
    public class PanelShopAction : IPanelAction
    {
        private AItemPanel _inventoryPanel;

        private IInventoryManager _inventory;
        private ItemSlotMouse _mousePanel;
        private ItemSlotInfo _itemSlot;

        public void Initialize(AItemPanel inventoryPanel, IInventoryManager inventory, ItemSlotMouse mousePanel, ItemSlotInfo itemSlot, Image itemImage, ItemSlotType slotType)
        {
            _inventoryPanel = inventoryPanel;
            _inventory = inventory;
            _mousePanel = mousePanel;
            _itemSlot = itemSlot;
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
                    }
                }

                else
                {
                    OnActionInventory();
                }
            }
        }

        private void PickupItem()
        {
            ItemSlotInfo tempSlot = new ItemSlotInfo(_itemSlot.item, 10);

            _mousePanel.itemSlot = tempSlot;
            _mousePanel.itemSlot.stacks = _itemSlot.item.maxStacks;
            _mousePanel.sourceItemPanel = _inventoryPanel;
            if (Input.GetKey(KeyCode.LeftShift) && _itemSlot.stacks > 1) _mousePanel.splitSize = _itemSlot.stacks / 2;
            else _mousePanel.splitSize = _itemSlot.stacks;
            _mousePanel.SetUI();
        }

        private void OnActionInventory()
        {
            //Clicked on original slot
            if (_itemSlot == _mousePanel.itemSlot)
            {
                _mousePanel.EmptySlot();
            }
            else
            {
                _mousePanel.EmptySlot();
            }

            _inventory.RefreshInventory();
        }

        public void RefreshInventory()
        {
            _inventory.RefreshInventory();
        }
    }
}
