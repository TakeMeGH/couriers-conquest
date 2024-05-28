using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CC.Inventory
{
    public class PanelInventoryAction : APanelComponent, IPanelAction
    {
        private PlayerInventoryManager _playerInventoryManager;
        private PanelInventory _panelInventory;
        public void Initialize(AItemPanel inventoryPanel, IInventoryManager inventory, ItemSlotMouse mousePanel, ItemSlotInfo itemSlot, Image itemImage, ItemSlotType slotType)
        {
            _inventoryPanel = inventoryPanel;
            _inventory = inventory;
            _mousePanel = mousePanel;
            _itemSlot = itemSlot;
            _itemImage = itemImage;
            _slotType = slotType;

            _panelInventory = (PanelInventory)inventoryPanel;
            _playerInventoryManager = (PlayerInventoryManager)inventory;
        }

        public void OnAction()
        {
            if (_inventory != null)
            {
                Debug.Log("Select Item");
                PickupItem();
                SetItemType(_itemSlot.item.GetItemType());

            }else
            {
                Debug.Log("Null Inventory");
            }
        }

        private void SetItemType(ItemType type)
        {
            Debug.Log(type.ToString());
            if (type == ItemType.Consumable)
            {
                _playerInventoryManager.activeSlot = ItemType.Consumable;
                CanEquipLabelUI();
                _playerInventoryManager.SetLabelConsumableType();
            }
            else if (type == ItemType.Rune)
            {
                _playerInventoryManager.activeSlot = ItemType.Rune;
                CanEquipLabelUI();
            }
            else if (type == ItemType.Equipment)
            {
                _playerInventoryManager.actionType = InventoryActionType.None;
                _playerInventoryManager.activeSlot = ItemType.Equipment;
            }
            else
            {
                _playerInventoryManager.actionType = InventoryActionType.None;
                _playerInventoryManager.activeSlot = ItemType.Materials;
                _playerInventoryManager.SetLabelMaterialsType();
            }
        }

        private void CanEquipLabelUI()
        {
            _playerInventoryManager.CanEquipSpesifikSlot();
        }

        public void RefreshInventory()
        {
            _inventory.RefreshInventory();
        }
    }
}
