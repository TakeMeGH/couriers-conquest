using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CC.Inventory
{
    public class PanelInventoryAction : APanelComponent, IPanelAction
    {

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
                    }
                    else if(_slotType == ItemSlotType.Rune)
                    {
                        OnActionRuneItem();
                    }
                    else if(_slotType == ItemSlotType.Equipment)
                    {
                        OnActionEquipmentSlot();
                    }
                }
            }
        }

        public void RefreshInventory()
        {
            _inventory.RefreshInventory();
        }
    }
}
