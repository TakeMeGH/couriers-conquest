using CC.Events;
using CC.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
using static UnityEditor.Progress;

namespace CC.UpgradeEquipment
{
    public class UpgradeEquipmentManager : MonoBehaviour
    {
        private AInventoryData _inventoryData;
        private InventoryData _playerInventoryData;
        private UpgradeEquipmentUI _upgradeEquipmentUI;
        private TempPlayerInventory _tempPlayerInventory;

        [Space]
        [Header("Upgrade Panel")]
        [SerializeField] private ItemSlotMouse _itemSlotMouse;
        [SerializeField] private AItemPanel _upgradeSlot;
        [SerializeField] private List<AItemPanel> _requirimentItemSlot = new List<AItemPanel>();
        private EquipmentItem _currentEquipmentItem;

        [Space]
        [Header("Events")]
        [SerializeField] private PlayerInventoryDataChannel _onShowUpgradeEvent;
        [SerializeField] private SellItemEventChannel _onSellItemEvent;
        [SerializeField] private OnUpdateCurrencyEventChannel _onUpdateCurrency;
        [SerializeField] private OnSenderBaseItemEventChannel _onUpgradeEquipment;

        private float _playerMoney = 0;
        private float _lastPrice = 0;

        private void OnEnable()
        {
            _upgradeEquipmentUI = GetComponent<UpgradeEquipmentUI>();
            _onShowUpgradeEvent.OnEventRaised += Initialize;
        }

        private void OnDisable()
        {
            _onShowUpgradeEvent.OnEventRaised -= Initialize;
        }

        private void Initialize(AInventoryData playerData, AInventoryData blacksmithInventoryData)
        {
            _inventoryData = blacksmithInventoryData;
            _playerInventoryData = (InventoryData)playerData;
            _upgradeEquipmentUI.ShowUpgradeUI();

            if (_tempPlayerInventory == null)
            {
                _tempPlayerInventory = GetComponentInChildren<TempPlayerInventory>();
                _upgradeEquipmentUI = GetComponent<UpgradeEquipmentUI>();

                _upgradeSlot.inventory = _tempPlayerInventory;
                _upgradeSlot.OnEnable();
                _upgradeEquipmentUI.Initialize(this);
                _upgradeEquipmentUI.SetButtonListener();
                SetRequirimentItemSlot();
            }

            ResetPanel();
            _tempPlayerInventory.Initialize(_inventoryData, this, _itemSlotMouse, _playerInventoryData);
        }

        public List<AItemPanel> GetUpgradePanel()
        {
            return _requirimentItemSlot;
        }

        private void SetRequirimentItemSlot()
        {
            foreach (AItemPanel itemPanel in _requirimentItemSlot)
            {
                itemPanel.inventory = _tempPlayerInventory;
                itemPanel.OnEnable();
            }
        }

        public AItemPanel GetEquipmentSlotPanel()
        {
            return _upgradeSlot;
        }

        private void ResetPanel()
        {
            _playerMoney = _playerInventoryData.playerGold;
            _upgradeEquipmentUI.UpdateTextPlayerMoney(_playerMoney);

            _upgradeSlot.itemSlot.item = null;
            foreach (AItemPanel panel in _requirimentItemSlot)
            {
                panel.itemSlot.item = null;
            }
            _upgradeEquipmentUI.SetUpgradeButton(false);
        }

        public bool CheckMaxLevelCondition(EquipmentItem item)
        {
            if(item == null)
            {
                _upgradeEquipmentUI.SetMaxLevelLabelPanel(false);
                return false;
            }

            if(item.weaponLevel < item.upgradeRequiriment.Count)
            {
                _upgradeEquipmentUI.SetMaxLevelLabelPanel(false);
                return false;
            }
            else
            {
                _upgradeEquipmentUI.SetMaxLevelLabelPanel(true);
                return true;
            }
        }

        public void UpdateCheckRequiriment(EquipmentItem item)
        {
            if (_upgradeSlot.itemSlot.item == null || CheckMaxLevelCondition(item))
            {
                _upgradeEquipmentUI.SetUpgradeButton(false);
                _lastPrice = 0;
                _upgradeEquipmentUI.UpdateTextPrice(_lastPrice);
                return;
            }

            _currentEquipmentItem = item;
            int level = item.weaponLevel;
            bool isUpgradeable = true;
            _lastPrice = item.upgradeRequiriment[level].price;
            _upgradeEquipmentUI.UpdateTextPrice(_lastPrice);

            for (int i = 0; i < item.upgradeRequiriment[level].materialRequiriment.Count; i++)
            {
                if (!(_requirimentItemSlot[i].itemSlot.item && _requirimentItemSlot[i].itemSlot.stacks >= item.upgradeRequiriment[level].materialRequiriment[i].amount) || _playerMoney < _lastPrice)
                {
                    isUpgradeable = false;
                    break;
                }
            }

            SetButtonCondition(isUpgradeable);
        }

        public void SetButtonCondition(bool condition)
        {
            _upgradeEquipmentUI.SetUpgradeButton(condition);
        }

        public void UpgradeEquipment()
        {
            UseMaterialRequiriment();
            ReducePlayerMoney();
            SuccesfullUpgrade();

            ResetPanel();
            _tempPlayerInventory.Initialize(_inventoryData, this, _itemSlotMouse, _playerInventoryData);
        }

        private void UpgradeLevelEquipment()
        {
            _onUpgradeEquipment.RaiseEvent(_currentEquipmentItem);
        }

        private void UseMaterialRequiriment()
        {
            int weaponLevel = _currentEquipmentItem.weaponLevel;
            for (int i = 0; i < _currentEquipmentItem.upgradeRequiriment[weaponLevel].materialRequiriment.Count; i++)
            {
                if (_requirimentItemSlot[i].itemSlot.item != null)
                {
                    _onSellItemEvent.RaiseEvent(_requirimentItemSlot[i].itemSlot.item, _currentEquipmentItem.upgradeRequiriment[weaponLevel].materialRequiriment[i].amount);
                }
            }
        }

        private void ReducePlayerMoney()
        {
            _onUpdateCurrency.RaiseEvent(-_lastPrice);
        }

        private void SuccesfullUpgrade()
        {
            _upgradeEquipmentUI.SetStatsPanelBeforeUpgrade(_currentEquipmentItem.itemSprite, _currentEquipmentItem.weaponLevel + 1, _currentEquipmentItem.attackWeapon, _currentEquipmentItem.deffWeapon, _currentEquipmentItem.itemWeight);
            UpgradeLevelEquipment();
            _upgradeEquipmentUI.SetStatsPanelAfterUpgrade(_currentEquipmentItem.itemSprite, _currentEquipmentItem.weaponLevel + 1, _currentEquipmentItem.attackWeapon, _currentEquipmentItem.deffWeapon, _currentEquipmentItem.itemWeight);
        }
    }
}