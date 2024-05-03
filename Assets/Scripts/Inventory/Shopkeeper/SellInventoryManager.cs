using CC.Events;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CC.Inventory
{
    public class SellInventoryManager : AShopInventoryManagement, IInventoryManager
    {

        [Header("Inventory Menu Components")]
        [Space]
        [SerializeField] private List<AItemPanel> _playerItemPanel;
        [SerializeField] private GameObject _playerInventoryPanelUI;

        [Space]
        [Header("Event Panel")]
        [SerializeField] ItemInventoryEventChannel _addItemToInventory;
        [SerializeField] SellItemEventChannel _onSellItemEvent;

        bool _onInitializeFirstTime = false;

        public void ShowPanel()
        {
            UnShowPlayerPanel();
            SetPanelShop();

            _inventoryData.items.Clear();
            CloneInventoryData(_playerInventoryData);

            Debug.Log("Sell Panel Show");
            for (int i = 0; i < _inventoryData.items.Count; i++)
            {
                if (_inventoryData.items[i].item)
                {
                    _playerItemPanel[i].gameObject.SetActive(true);
                    SetItemPanel(_playerItemPanel[i], _inventoryData.items[i]);

                }
            }
        }

        private void SetItemPanel(AItemPanel itemPanel, ItemSlotInfo slotInfo)
        {
            if (slotInfo != null)
            {
                itemPanel.itemSlot = new ItemSlotInfo(slotInfo.item, slotInfo.stacks);
                itemPanel.itemImage.sprite = slotInfo.item.itemSprite;
                itemPanel.itemImage.CrossFadeAlpha(1, 0.05f, true);
                itemPanel.name = slotInfo.item.itemName + " Panel";
                itemPanel.itemImage.gameObject.SetActive(true);
            }
            else
            {
                itemPanel.itemSlot = new ItemSlotInfo(null, 0);
            }

            itemPanel.mousePanel = _itemSlotMouse;
            itemPanel.inventory = this;
            itemPanel.OnEnable();
            RefreshInventory();
        }

        private void UnShowPlayerPanel()
        {
            for (int i = 0; i < _playerItemPanel.Count; i++)
            {
                _playerItemPanel[i].gameObject.SetActive(false);
            }
        }

        private void SetPanelShop()
        {
            for (int i = 0; i < _inventoryShopPanel.Count; i++)
            {
                SetItemPanel(_inventoryShopPanel[i], null);
            }
        }

        public void Initialize(AInventoryData inventoryData, ShopManager shopManager, ItemSlotMouse mousePanel, AInventoryData playerData)
        {
            _playerInventoryData = playerData;
            _inventoryData = inventoryData;
            _shopManager = shopManager;
            _itemSlotMouse = mousePanel;
            _inventoryManager = this;

            if (!_onInitializeFirstTime)
            {
                _inventoryShopPanel.Clear();
                _playerItemPanel.Clear();

                AItemPanel[] playerPanel = _playerInventoryPanelUI.GetComponentsInChildren<PanelInventory>();
                foreach (PanelInventory itemPanel in playerPanel)
                {
                    _playerItemPanel.Add(itemPanel);
                }


                AItemPanel[] shopPanel = _inventoryShopPanelUI.GetComponentsInChildren<PanelInventory>();
                foreach (PanelInventory itemPanel in shopPanel)
                {
                    _inventoryShopPanel.Add(itemPanel);
                }

                _onInitializeFirstTime = true;
            }

            SetPanelShop();
            UnShowPlayerPanel();
            UpdatePrice();
        }

        private void CloneInventoryData(AInventoryData playerData)
        {
            for (int i = 0; i < playerData.inventorySize; i++)
            {
                if (playerData.items[i].item)
                {
                    if (playerData.items[i].item.GetItemType() != ItemType.Equipment)
                        _inventoryData.items.Add(new ItemSlotInfo(_playerInventoryData.items[i].item, _playerInventoryData.items[i].stacks));
                }
            }
        }

        public void RefreshInventory()
        {
            RefreshedInventory(_inventoryShopPanel);
            RefreshedInventory(_playerItemPanel);
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

            _shopManager.UpdateSellPrice(price);
        }

        public void SellItem()
        {
            Debug.Log("One Time Buy");
            for (int i = 0; i < _inventoryShopPanel.Count; i++)
            {
                if (_inventoryShopPanel[i].itemSlot.item != null)
                {
                    _onSellItemEvent.RaiseEvent(_inventoryShopPanel[i].itemSlot.item, _inventoryShopPanel[i].itemSlot.stacks);
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

