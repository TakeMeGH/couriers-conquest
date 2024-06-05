using CC.Events;
using CC.Shop;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CC.Inventory
{
    public class SellManager : MonoBehaviour
    {
        public List<PanelInventorySell> existingPanels = new List<PanelInventorySell>();
        [SerializeField] private InputReader _inputReader;
        private ShopManager _shopManager;
        private InventoryData _inventoryData;
        private ItemSlotMouse _itemSlotMouse;

        private int _previousPanelIndex;
        public int activePanelIndex;

        [Header("UI COMPONENT")]
        [SerializeField] private TextMeshProUGUI _textPlayerGold;
        [SerializeField] private Slider _amountSellSlider;
        [SerializeField] private TextMeshProUGUI _textAmountQuantity;
        [SerializeField] private TextMeshProUGUI _textCost;
        [SerializeField] private TextMeshProUGUI _textNotificationFailed;

        [Header("PANEL UI")]
        [SerializeField] private GameObject _inventoryPanel;
        [SerializeField] private GameObject _itemDetailPanel;
        [SerializeField] private GameObject _labelConfirm;
        [SerializeField] private GameObject _panelDetailSell;
        [SerializeField] private GameObject _failedSellPanel;

        [Header("SPRITE SELECTED AND DESELECTED")]
        [SerializeField] private Sprite _frameItemDefault;
        [SerializeField] private Sprite _frameItemHover;

        private int _costSell;
        private int _amountSell;
        private string _stringNotification;
        [SerializeField] private bool _isSelected;
        [SerializeField] private bool _hasSelectedUI;

        [Header("Event Panel")]
        [SerializeField] private OnUpdateCurrencyEventChannel _updateCurrencyEventChannel;

        public void InitComponent()
        {
            gameObject.SetActive(true);
            PanelInventorySell[] itemPanelsInGrid = _inventoryPanel.GetComponentsInChildren<PanelInventorySell>();

            foreach (PanelInventorySell panel in itemPanelsInGrid)
            {
                existingPanels.Add(panel);
            }

            gameObject.SetActive(false);
            InitUI();
        }

        private void InitUI()
        {
            _itemDetailPanel.SetActive(false);
            _labelConfirm.SetActive(false);
            _panelDetailSell.SetActive(false);
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

        public void Initialize(AInventoryData inventoryData, ShopManager shopManager, ItemSlotMouse mousePanel)
        {
            _inventoryData = (InventoryData)inventoryData;
            _shopManager = shopManager;
            _itemSlotMouse = mousePanel;

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
            for (int i = 0; i < _inventoryData.inventoryIndex; i++)
            {
                if (_inventoryData.items[i].item == null)
                {
                    existingPanels[i].Initialize(this, i, null, _frameItemDefault, _frameItemHover);
                    existingPanels[i].HideEquipIcon();
                }
                else
                {
                    if ((_inventoryData.isPouchEquiped && i == _inventoryData.indexPouchEquiped) || (_inventoryData.isRuneEquiped && i == _inventoryData.indexRuneEquiped))
                    {
                        existingPanels[i].Initialize(this, i, _inventoryData.items[i], _frameItemDefault, _frameItemHover);
                        existingPanels[i].ShowEquipIcon();
                    }
                    else
                    {
                        existingPanels[i].Initialize(this, i, _inventoryData.items[i], _frameItemDefault, _frameItemHover);
                        existingPanels[i].HideEquipIcon();
                    }
                }
            }
        }

        public void SwapSelected(int targetSelected)
        {
            _previousPanelIndex = activePanelIndex;
            activePanelIndex = targetSelected;
            existingPanels[_previousPanelIndex].CancelSelected();
            SelectItem();
        }

        private void ConfirmAction()
        {
            AudioManager.instance.AudioPlayOneShot(AudioManager.instance.ConfirmUI, transform.position);
            _hasSelectedUI = true;

            if (IsSellable())
            {
                _panelDetailSell.SetActive(true);
                SetSlider();
            }
            else
            {
                _textNotificationFailed.text = _stringNotification;
                _failedSellPanel.SetActive(true);
            }
        }

        private bool IsSellable()
        {
            if (_inventoryData.items[activePanelIndex].item.GetItemType() == ItemType.QuestItem)
            {
                _stringNotification = "Tidak Dapat Menjual Item Quest";
                return false;
            }
            else if ((_inventoryData.isPouchEquiped && activePanelIndex == _inventoryData.indexPouchEquiped) || (_inventoryData.isRuneEquiped && activePanelIndex == _inventoryData.indexRuneEquiped))
            {
                _stringNotification = "Tidak Dapat Menjual Item Yang di Equip";
                return false;
            }
            else
            {
                return true;
            }
        }

        public void SelectItem()
        {
            _itemSlotMouse.itemSlot = _inventoryData.items[activePanelIndex];
            _itemSlotMouse.SetUI();
            _isSelected = true;
            _amountSell = 0;

            _itemDetailPanel.SetActive(true);
            _labelConfirm.SetActive(true);
        }

        private void SetSlider()
        {
            _amountSellSlider.value = 0;
            _amountSellSlider.maxValue = _inventoryData.items[activePanelIndex].stacks;
            _textCost.text = "0";
        }

        public void PlusAmount()
        {
            _amountSellSlider.value++;
        }

        public void MinusAmount()
        {
            _amountSellSlider.value--;
        }

        public void UpdateCost()
        {
            _amountSell = (int)_amountSellSlider.value;
            _costSell = _inventoryData.items[activePanelIndex].item.costSell * _amountSell;
            _textAmountQuantity.text = _amountSell.ToString();
            _textCost.text = _costSell.ToString();
        }

        public void AttempToSell()
        {
            _panelDetailSell.SetActive(false);

            ReduceItem();
        }

        public void BackAction()
        {
            if (_hasSelectedUI)
            {
                _panelDetailSell.SetActive(false);
                _failedSellPanel.SetActive(false);
                _hasSelectedUI = false;
            }
            else
            {
                AudioManager.instance.AudioPlayOneShot(AudioManager.instance.BackUI, transform.position);
                gameObject.SetActive(false);
                _inputReader.EnableGameplayInput();
                _shopManager.ShowOtherUI();
            }
        }

        private void ReduceItem()
        {
            _inventoryData.items[activePanelIndex].stacks -= _amountSell;

            if (_inventoryData.items[activePanelIndex].stacks <= 0)
            {
                _inventoryData.items[activePanelIndex].item = null;
            }
            SellSucces();
        }

        private void SellSucces()
        {
            _updateCurrencyEventChannel.RaiseEvent(_costSell);
            BackAction();
            _textPlayerGold.text = _inventoryData.playerGold.ToString();
            ShowPanel();
        }
    }
}
