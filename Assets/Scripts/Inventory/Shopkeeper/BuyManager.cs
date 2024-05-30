using CC.Events;
using CC.Shop;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CC.Inventory
{
    public class BuyManager : MonoBehaviour
    {
        private ShopManager _shopManager;
        private InventoryData _inventoryData;
        private ItemSlotMouse _itemSlotMouse;
        private ItemShopkeeper _itemShopkeeper;
        [SerializeField] private InputReader _inputReader;
        public int activePanelIndex;
        [SerializeField] private List<BuyItemPanel> _buyPanelList = new();

        [Header("UI COMPONENT")]
        [SerializeField] private TextMeshProUGUI _textPlayerGold;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private Slider _amountBuySlider;
        [SerializeField] private TextMeshProUGUI _textAmountQuantity;
        [SerializeField] private TextMeshProUGUI _textCost;
        [SerializeField] private TextMeshProUGUI _textNotificationFailed;

        [Header("PANEL UI")]
        [SerializeField] private GameObject _buyPanel;
        [SerializeField] private GameObject _itemDetailPanel;
        [SerializeField] private GameObject _labelConfirm;
        [SerializeField] private GameObject _panelDetailBuy;
        [SerializeField] private GameObject _failedBuyPanel;

        [Header("SPRITE SELECTED AND DESELECTED")]
        [SerializeField] private Sprite _frameBackgroundDefault;
        [SerializeField] private Sprite _frameItemDefault;
        [SerializeField] private Sprite _frameCostDefault;
        [SerializeField] private Sprite _frameBackgroundHover;
        [SerializeField] private Sprite _frameItemHover;
        [SerializeField] private Sprite _frameCostHover;

        [Header("Event Panel")]
        [SerializeField] private ItemInventoryEventChannel _addItemToInventory;
        [SerializeField] private OnUpdateCurrencyEventChannel _updateCurrencyEventChannel;

        private int _costBuy;
        [SerializeField] private bool _isSelected;
        [SerializeField] private bool _hasSelectedUI;
        private int _amountBuy = 0;

        public void InitComponent()
        {
            gameObject.SetActive(true);
            BuyItemPanel[] itemPanelsInGrid = _buyPanel.GetComponentsInChildren<BuyItemPanel>();

            foreach (BuyItemPanel panel in itemPanelsInGrid)
            {
                _buyPanelList.Add(panel);
                panel.gameObject.SetActive(false);
            }

            gameObject.SetActive(false);
            InitUI();
        }

        private void OnEnable()
        {
            _inputReader.ConfirmActionPerformed += ConfirmAction;
            _inputReader.BackActionPerformed += BackAction;
        }

        private void OnDisable()
        {
            _inputReader.ConfirmActionPerformed -= ConfirmAction;
            _inputReader.BackActionPerformed -= BackAction;
        }
        public void Initialize(AInventoryData inventoryData, ShopManager shopManager, ItemSlotMouse mousePanel, ItemShopkeeper itemData)
        {
            _inventoryData = (InventoryData)inventoryData;
            _shopManager = shopManager;
            _itemSlotMouse = mousePanel;
            _itemShopkeeper = itemData;

            _isSelected = false;
            _hasSelectedUI = false;

            if (_inventoryData == null)
            {
                Debug.Log("Null");
            }

            _textPlayerGold.text = _inventoryData.playerGold.ToString();

            ShowPanel();
            InitUI();
        }

        public void ShowPanel()
        {
            for (int i = 0; i < _itemShopkeeper.listItem.Count; i++)
            {
                _buyPanelList[i].Initialize(this, i);
                _buyPanelList[i].SetUI(_itemShopkeeper.listItem[i].item.itemSprite, _itemShopkeeper.listItem[i].item.itemName, _itemShopkeeper.listItem[i].item.costBuy.ToString());
                _buyPanelList[i].gameObject.SetActive(true);
            }
            ScrollToTopPosition();
        }

        public void BackAction()
        {
            if (_hasSelectedUI)
            {
                _panelDetailBuy.SetActive(false);
                _failedBuyPanel.SetActive(false);
                _hasSelectedUI = false;
            }
            else
            {
                gameObject.SetActive(false);
                _inputReader.EnableGameplayInput();
                _shopManager.ShowOtherUI();
            }
        }


        private void InitUI()
        {
            _itemDetailPanel.SetActive(false);
            _labelConfirm.SetActive(false);
            _panelDetailBuy.SetActive(false);
            _failedBuyPanel.SetActive(false);
        }

        public void SelectItem()
        {
            _itemSlotMouse.itemSlot = _itemShopkeeper.listItem[activePanelIndex];
            _itemSlotMouse.SetUI();
            _isSelected = true;

            _amountBuy = 0;

            _itemDetailPanel.SetActive(true);
            _labelConfirm.SetActive(true);
        }

        private void ConfirmAction()
        {
            _panelDetailBuy.SetActive(true);
            _hasSelectedUI = true;
            SetSlider();
        }

        public void SetSelectedPanel()
        {
            DeselectAllBuyPanel();
            _buyPanelList[activePanelIndex].SetSelectedPanel(_frameBackgroundHover, _frameItemHover, _frameCostHover);
        }

        private void ScrollToTopPosition()
        {
            if (_scrollRect != null)
            {
                _scrollRect.verticalNormalizedPosition = 1f;
            }
        }

        private void SetSlider()
        {
            _amountBuySlider.value = 0;
            _amountBuySlider.maxValue = _itemShopkeeper.listItem[activePanelIndex].item.maxStacks;
            _textCost.text = "0";
        }

        public void PlusAmount()
        {
            _amountBuySlider.value++;
        }

        public void MinusAmount()
        {
            _amountBuySlider.value--;
        }

        public void UpdateCost()
        {
            _amountBuy = (int)_amountBuySlider.value;
            _costBuy = _itemShopkeeper.listItem[activePanelIndex].item.costBuy * _amountBuy;
            _textAmountQuantity.text = _amountBuy.ToString();
            _textCost.text = _costBuy.ToString();

            if (CanBuyItem())
            {
                _textCost.color = Color.white;
            }
            else
            {
                _textCost.color = Color.red;
            }
        }

        public void AttempToBuy()
        {
            _panelDetailBuy.SetActive(false);

            if (CanBuyItem())
            {
                if (CheckInventoryAvaibilty(_itemShopkeeper.listItem[activePanelIndex].item, _amountBuy))
                {
                    BuySucces();
                    _hasSelectedUI = false;
                }
                else
                {
                    _textNotificationFailed.text = "Slot Inventory Tidak Cukup";
                    _failedBuyPanel.SetActive(true);
                }
            }
            else
            {
                _textNotificationFailed.text = "Gold Kamu Tidak Cukup";
                _failedBuyPanel.SetActive(true);
            }
        }

        private void BuySucces()
        {
            _addItemToInventory.RaiseEvent(_itemShopkeeper.listItem[activePanelIndex].item, _amountBuy);
            _updateCurrencyEventChannel.RaiseEvent(-_costBuy);
            BackAction();
            _textPlayerGold.text = _inventoryData.playerGold.ToString();
        }

        private bool CanBuyItem()
        {
            if (_inventoryData.playerGold > _costBuy)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void DeselectAllBuyPanel()
        {
            foreach (BuyItemPanel panel in _buyPanelList)
            {
                panel.SetDeselectedPanel(_frameBackgroundDefault, _frameItemDefault, _frameCostDefault);
            }
        }

        public bool CheckInventoryAvaibilty(ABaseItem item, int amount)
        {
            for (int i = 0; i < _inventoryData.inventoryIndex; i++)
            {
                if (_inventoryData.items[i].item == null)
                {
                    return true;
                }
            }

            for (int i = 0; i < _inventoryData.inventoryIndex; i++)
            {
                if (_inventoryData.items[i].item != null)
                {
                    if (_inventoryData.items[i].item == item)
                    {
                        if (amount <= 0) return true;

                        if (_inventoryData.items[i].stacks < _inventoryData.items[i].item.maxStacks)
                        {
                            amount -= (_inventoryData.items[i].item.maxStacks - _inventoryData.items[i].stacks);
                        }
                    }
                }
            }

            if(amount <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
