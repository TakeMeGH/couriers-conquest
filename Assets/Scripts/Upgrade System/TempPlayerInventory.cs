using CC.Events;
using CC.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

namespace CC.UpgradeEquipment
{
    public class TempPlayerInventory : AInventoryManagement, IInventoryManager
    {
        private UpgradeEquipmentManager _upgradeEquipmentManager;
        [SerializeField] private List<AItemPanel> _upgradeItempanels = new List<AItemPanel>();
        private AItemPanel _equipmentSlotPanel;
        [SerializeField] private ABaseItem _lastItem = null;
        private EquipmentItem _eqItem;
        private List<UpgradeMaterialRequiriment> _lastRequirimentItems = new List<UpgradeMaterialRequiriment>();

        [Space]
        [Header("Event Panel")]
        [SerializeField] ItemInventoryEventChannel _addItemToInventory;
        [SerializeField] SellItemEventChannel _onSellItemEvent;

        [SerializeField] private bool _onInitializeFirstTime = false;


        public void ShowPanel()
        {
            UnShowPlayerPanel();
            SetPanelShop();

            _inventoryData.items.Clear();
            CloneInventoryData(_playerInventoryData);

            for (int i = 0; i < _inventoryData.items.Count; i++)
            {
                if (_inventoryData.items[i].item)
                {
                    _inventoryItemPanel[i].gameObject.SetActive(true);
                    SetItemPanel(_inventoryItemPanel[i], _inventoryData.items[i]);
                }
            }

            RefreshInventory();
        }

        private void UnShowPlayerPanel()
        {
            for (int i = 0; i < _inventoryItemPanel.Count; i++)
            {
                _inventoryItemPanel[i].gameObject.SetActive(false);
            }
        }

        private void SetPanelShop()
        {
            for (int i = 0; i < _inventoryItemPanel.Count; i++)
            {
                SetItemPanel(_inventoryItemPanel[i], null);
            }
        }

        public void Initialize(AInventoryData inventoryData, UpgradeEquipmentManager upgradeEquipmentManager, ItemSlotMouse mousePanel, AInventoryData playerData)
        {
            _inventoryManager = this;
            _playerInventoryData = playerData;
            _inventoryData = inventoryData;
            _upgradeEquipmentManager = upgradeEquipmentManager;
            _itemSlotMouse = mousePanel;
            _inventoryManager = this;

            if (!_onInitializeFirstTime)
            {
                _inventoryItemPanel.Clear();

                AItemPanel[] tempPlayerPanel = _inventoryPanelUI.GetComponentsInChildren<PanelInventory>();
                foreach (PanelInventory itemPanel in tempPlayerPanel)
                {
                    _inventoryItemPanel.Add(itemPanel);
                }

                _onInitializeFirstTime = true;
                SetUpgradeItemPanels();
            }

            ShowPanel();
            UpdatePrice();
            RefreshAllPanel();
        }

        private void SetUpgradeItemPanels()
        {
            _equipmentSlotPanel = _upgradeEquipmentManager.GetEquipmentSlotPanel();
            _upgradeItempanels = _upgradeEquipmentManager.GetUpgradePanel();
        }

        private void CloneInventoryData(AInventoryData playerData)
        {
            for (int i = 0; i < playerData.items.Count; i++)
            {
                if (playerData.items[i].item)
                {
                    if (playerData.items[i].item.GetItemType() != ItemType.Consumable)
                        _inventoryData.items.Add(new ItemSlotInfo(_playerInventoryData.items[i].item, _playerInventoryData.items[i].stacks));
                }
            }
        }

        public void RefreshInventory()
        {
            RefreshedInventory(_inventoryItemPanel);

            if (_equipmentSlotPanel.itemSlot.item != null)
            {
                if (_lastItem == null || _lastItem != _equipmentSlotPanel.itemSlot.item)
                {
                    AddExistingRequiriment();
                    RefreshEquipmentSlotPanel(_equipmentSlotPanel);
                    _lastItem = _equipmentSlotPanel.itemSlot.item;
                    SetUpgradeRequiriment(_equipmentSlotPanel.itemSlot.item);
                    _upgradeEquipmentManager.UpdateCheckRequiriment(_eqItem);
                }
                else
                {
                    RefreshRequirimentPanel();
                    RefreshEquipmentSlotPanel(_equipmentSlotPanel);
                    _upgradeEquipmentManager.UpdateCheckRequiriment(_eqItem);
                }

            }
            else if (_equipmentSlotPanel.itemSlot.item == null)
            {
                AddExistingRequiriment();
                _lastItem = null;
                _eqItem = null;
                RefreshEquipmentSlotPanel(_equipmentSlotPanel);
                UnShowPanelRequiriment();
                _upgradeEquipmentManager.UpdateCheckRequiriment(null);
                if (_upgradeEquipmentManager.CheckMaxLevelCondition(_eqItem)) return;
            }

            UpdatePrice();
        }

        private void SetUpgradeRequiriment(ABaseItem item)
        {
            UnShowPanelRequiriment();
            _lastRequirimentItems = new List<UpgradeMaterialRequiriment>();
            _eqItem = (EquipmentItem)item;

            if (_upgradeEquipmentManager.CheckMaxLevelCondition(_eqItem)) return;

            int level = _eqItem.weaponLevel;
            int amountKindRequiriment = _eqItem.upgradeRequiriment[level].materialRequiriment.Count;
            for (int i = 0; i < amountKindRequiriment; i++)
            {
                UpgradeEquipmentPanel panelScript = (UpgradeEquipmentPanel)_upgradeItempanels[i];
                panelScript.gameObject.SetActive(true);
                panelScript.SetSpesifikRequiriment(_eqItem.upgradeRequiriment[level].materialRequiriment[i]);
                _lastRequirimentItems.Add(_eqItem.upgradeRequiriment[level].materialRequiriment[i]);
                RefreshRequirimentPanel();
            }
        }
        private void RefreshRequirimentPanel()
        {
            for (int i = 0; i < _lastRequirimentItems.Count; i++)
            {
                RefreshRequirimentPanel(_upgradeItempanels[i], _lastRequirimentItems[i]);
            }
        }

        private void RefreshAllPanel()
        {
            _lastItem = null;
            UnShowPanelRequiriment();
            RefreshEquipmentSlotPanel(_equipmentSlotPanel);
            RefreshRequirimentPanel();
        }

        private void UnShowPanelRequiriment()
        {
            foreach (AItemPanel item in _upgradeItempanels)
            {
                item.gameObject.SetActive(false);
            }
        }

        private void AddExistingRequiriment()
        {
            for (int i = 0; i < _upgradeItempanels.Count; i++)
            {
               if (_upgradeItempanels[i].itemSlot.item != null)
               {
                    OnAddItem(_upgradeItempanels[i].itemSlot.item, _upgradeItempanels[i].itemSlot.stacks, _inventoryItemPanel);
                    _upgradeItempanels[i].itemSlot.item = null;
               }
            }
            RefreshedInventory(_inventoryItemPanel);
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
        }

        public void SellItem()
        {
            for (int i = 0; i < _inventoryItemPanel.Count; i++)
            {
                if (_inventoryItemPanel[i].itemSlot.item != null)
                {
                    _onSellItemEvent.RaiseEvent(_inventoryItemPanel[i].itemSlot.item, _inventoryItemPanel[i].itemSlot.stacks);
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
