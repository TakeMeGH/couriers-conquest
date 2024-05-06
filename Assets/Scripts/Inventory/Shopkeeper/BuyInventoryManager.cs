using CC.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

namespace CC.Inventory
{
    public class BuyInventoryManager : AInventoryManagement, IInventoryManager
    {
        [SerializeField] private bool _firstInit = false;
        private ShopManager _shopManager;

        [Header("Event Panel")]
        [SerializeField] private ItemInventoryEventChannel _addItemToInventory;

        public void ShowPanel()
        {
            UnShopPanel();

            for (int i = 0; i < _inventoryItemPanel.Count; i++)
            {
                if (!_playerInventoryData.items[i].item)
                {
                    _inventoryItemPanel[i].gameObject.SetActive(true);
                    SetItemPanel(_inventoryItemPanel[i], null);
                }
            }

            RefreshInventory();
        }

        private void UnShopPanel()
        {
            for (int i = 0; i < _inventoryItemPanel.Count; i++)
            {
                 _inventoryItemPanel[i].gameObject.SetActive(false);
            }
        }

        public void Initialize(AInventoryData inventoryData, ShopManager shopManager, ItemSlotMouse mousePanel, AInventoryData playerData)
        {
            if (!_firstInit)
            {
                _inventoryData = inventoryData;
                _shopManager = shopManager;
                _itemSlotMouse = mousePanel;
                _playerInventoryData = playerData;
                _inventoryManager = this;

                _inventoryItemPanel.Clear();
                AItemPanel[] Script = _inventoryPanelUI.GetComponentsInChildren<PanelInventoryBuy>();

                foreach (PanelInventoryBuy itemPanel in Script)
                {
                    _inventoryItemPanel.Add(itemPanel);
                }

                _firstInit = true;
            }

            UnShopPanel();
            _inventoryData.items.Clear();
            UpdatePrice();
        }

        public void RefreshInventory()
        {
            RefreshedInventory(_inventoryItemPanel);
            UpdatePrice();
        }

        private void UpdatePrice()
        {
            float price = 0;
            for (int i = 0; i < _inventoryItemPanel.Count; i++)
            {
                ABaseItem item = _inventoryItemPanel[i]?.itemSlot?.item;
                if (item != null)
                {
                    price += item.costSell * item.maxStacks;
                }
            }

            _shopManager.UpdateBuyPrice(price);
        }

        public void BuyItem()
        {
            Debug.Log("One Time Buy");
            for (int i = 0; i < _inventoryItemPanel.Count; i++)
            {
                if (_inventoryItemPanel[i].itemSlot.item != null)
                {
                    _addItemToInventory.RaiseEvent(_inventoryItemPanel[i].itemSlot.item, _inventoryItemPanel[i].itemSlot.stacks);
                    _inventoryItemPanel[i].itemSlot.item = null;
                    _inventoryItemPanel[i].itemSlot.stacks = 0;
                }
            }

            UpdatePrice();
            ShowPanel();
            RefreshInventory();
        }

        public void ClearSlot(ItemSlotInfo slot)
        {
            slot.item = null;
            slot.stacks = 0;
        }
    }
}
