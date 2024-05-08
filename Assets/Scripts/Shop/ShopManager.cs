using CC.Events;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace CC.Inventory
{
    public class ShopManager : MonoBehaviour
    {
        [SerializeField] InputReader _inputReader;
        private AInventoryData _inventoryData;
        private InventoryShopkeeper _inventoryShop;
        private InventoryData _playerData;

        [SerializeField] private ItemSlotMouse _itemSlotMouse;
        private BuyInventoryManager _shopInventoryManager;
        private SellInventoryManager _sellInventoryManager;

        [SerializeField] private List<UIPanelShop> _panelScript = new List<UIPanelShop>();
        [SerializeField] private List<AItemPanel> _itemPanel = new List<AItemPanel>();

        [Space]
        [Header("Button Action")]
        [SerializeField] private Button _buyMenu;
        [SerializeField] private Button _sellMenu;

        [Space]
        [Header("BUY UI Component")]
        [SerializeField] private TextMeshProUGUI _textMoney;
        [SerializeField] private TextMeshProUGUI _textPrice;
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _cancelBuyButton;
        [SerializeField] private GameObject _shopCanvas;

        [Space]
        [Header("Sell UI Component")]
        [SerializeField] private TextMeshProUGUI _textSell;
        [SerializeField] private Button _sellButton;
        [SerializeField] private Button _cancelSellButton;

        private bool _isToogle = false;
        private float _lastPrice = 0;

        [Header("Event Panel")]
        [SerializeField] private PlayerInventoryDataChannel _onEventInventory;
        [SerializeField] private OnUpdateCurrencyEventChannel _onUpdateCurrency;

        private void OnEnable()
        {
            _onEventInventory.OnEventRaised += Initialize;
        }

        private void OnDisable()
        {
            _onEventInventory.OnEventRaised -= Initialize;
        }

        private void Initialize(AInventoryData playerData, AInventoryData shopkeeperInventoryData)
        {
            _inventoryData = shopkeeperInventoryData;
            _playerData = (InventoryData)playerData;
            _shopCanvas.SetActive(true);
            GetAllPanel();
            _inputReader.EnableInventoryUIInput();


            if (_shopInventoryManager == null)
            {
                _shopInventoryManager = GetComponentInChildren<BuyInventoryManager>();
                _sellInventoryManager = GetComponentInChildren<SellInventoryManager>();

                _buyButton.onClick.RemoveAllListeners();
                _buyButton.onClick.AddListener(BuyItem);

                _buyMenu.onClick.AddListener(ToogleMenu);
                _sellMenu.onClick.AddListener(ToogleMenu);

                _sellButton.onClick.RemoveAllListeners();
                _sellButton.onClick.AddListener(SellItem);
            }

            _inventoryShop = (InventoryShopkeeper)_inventoryData;
            _shopInventoryManager.Initialize(_inventoryData, this, _itemSlotMouse, playerData);
            _sellInventoryManager.Initialize(_inventoryData, this, _itemSlotMouse, playerData);


            ToogleMenu();
            ShowPanel();

        }

        public void ClosePanel()
        {
            _isToogle = false;
            _shopInventoryManager.gameObject.SetActive(_isToogle);
            _sellInventoryManager.gameObject.SetActive(_isToogle);
            _shopCanvas.SetActive(_isToogle);
            _inputReader.EnableGameplayInput();
        }

        private void GetAllPanel()
        {
            UIPanelShop[] Script = GetComponentsInChildren<UIPanelShop>();

            // Menambahkan semua script ItemPanel ke dalam list
            foreach (UIPanelShop itemPanel in Script)
            {
                _panelScript.Add(itemPanel);
                _itemPanel.Add(itemPanel.GetComponentInChildren<AItemPanel>());

                itemPanel.gameObject.SetActive(false);
            }
        }

        private void SetItemPanel(AItemPanel itemPanel, ABaseItem item)
        {
            itemPanel.mousePanel = _itemSlotMouse;
            itemPanel.itemSlot = new ItemSlotInfo(item, 10);
            itemPanel.itemImage.sprite = item.itemSprite;
            itemPanel.itemImage.CrossFadeAlpha(1, 0.05f, true);
            itemPanel.name = item.itemName + " Panel";
            itemPanel.itemImage.gameObject.SetActive(true);

            itemPanel.inventory = _shopInventoryManager;
            itemPanel.OnEnable();
        }

        private void ShowPanel()
        {
            for (int i = 0; i < _inventoryShop.sellItem.Count; i++)
            {
                _inventoryData.items.Add(new ItemSlotInfo(_inventoryShop.sellItem[i], 10));

                _panelScript[i].Initialize(_inventoryData.items[i].item);
                _panelScript[i].gameObject.SetActive(true);

                SetItemPanel(_itemPanel[i], _inventoryShop.sellItem[i]);
            }
        }

        private void ToogleMenu()
        {
            _isToogle = !_isToogle;
            _buyMenu.interactable = !_isToogle;
            _sellMenu.interactable = _isToogle;

            _shopInventoryManager.gameObject.SetActive(_isToogle);
            _sellInventoryManager.gameObject.SetActive(!_isToogle);

            if (_isToogle)
            {
                _shopInventoryManager.ShowPanel();
            }
            else
            {
                _sellInventoryManager.ShowPanel();

            }
        }

        public void UpdateBuyPrice(float amount)
        {
            _lastPrice = 0;
            _lastPrice = amount;
            SetButton(_lastPrice != 0 && _playerData.playerGold >= amount);

            _textPrice.text = " - " +  _lastPrice.ToString();
            _textMoney.text = _playerData.playerGold.ToString();
        }

        public void UpdateSellPrice(float amount)
        {
            _lastPrice = 0;
            _lastPrice = amount;
            SetButton(_lastPrice != 0);

            _textSell.text = " + " + amount.ToString();
        }

        private void SetButton(bool condition)
        {
            _buyButton.interactable = condition;
            _sellButton.interactable = condition;
        }

        private void BuyItem()
        {
            _onUpdateCurrency.RaiseEvent(-_lastPrice);
            _shopInventoryManager.BuyItem();
        }

        private void SellItem()
        {
            _onUpdateCurrency.RaiseEvent(_lastPrice);
            _sellInventoryManager.SellItem();
        }
    }
}
