using CC.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CC.UpgradeEquipment
{
    public class PanelUpgradeEquipmentAction : APanelComponent, IPanelAction
    {
        [SerializeField] private bool _isRequiriment = false;
        [SerializeField] UpgradeMaterialRequiriment _materialRequiriment = null;

        public void Initialize(AItemPanel inventoryPanel, IInventoryManager inventory, ItemSlotMouse mousePanel, ItemSlotInfo itemSlot, Image itemImage, ItemSlotType slotType)
        {
            _inventoryPanel = inventoryPanel;
            _inventory = inventory;
            _mousePanel = mousePanel;
            _itemSlot = itemSlot;
            _itemImage = itemImage;
            _slotType = slotType;
        }

        public void SetRequiriment(bool value)
        {
            _isRequiriment = value;
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
                    if (_isRequiriment)
                    {
                        if (_mousePanel.itemSlot.item == _materialRequiriment.itemRequiriment)
                        {
                            //OnActionInventory();
                        }
                        else
                        {
                            Debug.Log("Not Match");
                        }
                    }
                    else
                    {

                        if (_mousePanel.itemSlot.item.GetItemType() == ItemType.Equipment)
                        {
                            //OnActionInventory();
                        }
                        else
                        {
                            Debug.Log("Not Match");
                            Debug.Log(_isRequiriment.ToString());
                        }
                    }
                }
            }
        }

        public void SetSpesifikRequiriment(UpgradeMaterialRequiriment item)
        {
            Debug.Log("set spesifik");
            _materialRequiriment = new UpgradeMaterialRequiriment();
            _materialRequiriment = item;
        }

        public void RefreshInventory()
        {
            _inventory.RefreshInventory();
        }
    }
}
