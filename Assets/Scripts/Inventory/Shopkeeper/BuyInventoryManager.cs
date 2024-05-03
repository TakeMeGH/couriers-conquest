using CC.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Inventory
{
    public class BuyInventoryManager : AShopInventoryManagement, IInventoryManager
    {
        [Header("Event Panel")]
        [SerializeField] private ItemInventoryEventChannel _addItemToInventory;

        public void ShowPanel()
        {
            UnShopPanel();

            for (int i = 0; i < _inventoryShopPanel.Count; i++)
            {
                if (!_playerInventoryData.items[i].item)
                {
                    _inventoryShopPanel[i].gameObject.SetActive(true);
                    SetItemPanel(_inventoryShopPanel[i]);
                }
            }
        }

        private void SetItemPanel(AItemPanel itemPanel)
        {
            itemPanel.mousePanel = _itemSlotMouse;
            itemPanel.itemSlot = new ItemSlotInfo(null, 0);

            itemPanel.inventory = this;
            itemPanel.OnEnable();
        }

        private void UnShopPanel()
        {
            for (int i = 0; i < _inventoryShopPanel.Count; i++)
            {
                 _inventoryShopPanel[i].gameObject.SetActive(false);
            }
        }

        public void Initialize(AInventoryData inventoryData, ShopManager shopManager, ItemSlotMouse mousePanel, AInventoryData playerData)
        {
            _inventoryData = inventoryData;
            _shopManager = shopManager;
            _itemSlotMouse = mousePanel;
            _playerInventoryData = playerData;
            _inventoryManager = this;

            _inventoryShopPanel.Clear();
            AItemPanel[] Script = _inventoryShopPanelUI.GetComponentsInChildren<PanelInventoryBuy>();

            foreach (PanelInventoryBuy itemPanel in Script)
            {
                _inventoryShopPanel.Add(itemPanel);
            }

            UnShopPanel();
            _inventoryData.items.Clear();
            UpdatePrice();
        }

        public void RefreshInventory()
        {
            RefreshedInventory(_inventoryShopPanel);
            UpdatePrice();
        }

        private void UpdatePrice()
        {
            float price = 0;
            for (int i = 0; i < _inventoryShopPanel.Count; i++)
            {
                ABaseItem item = _inventoryShopPanel[i]?.itemSlot?.item;
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
            for (int i = 0; i < _inventoryShopPanel.Count; i++)
            {
                if (_inventoryShopPanel[i].itemSlot.item != null)
                {
                    _addItemToInventory.RaiseEvent(_inventoryShopPanel[i].itemSlot.item, _inventoryShopPanel[i].itemSlot.stacks);
                    _inventoryShopPanel[i].itemSlot.item = null;
                    _inventoryShopPanel[i].itemSlot.stacks = 0;
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
