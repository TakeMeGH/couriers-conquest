using CC.Events;
using CC.Inventory;
using CC.Shop;
using CC.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace CC.UpgradeEquipment
{
    public class BlacksmithManager : MonoBehaviour
    {
        private UIUpgradeEquipment _upgradeEquipment;
        [SerializeField] private List<PanelUpgrade> _panelInventory = new List<PanelUpgrade>();
        [SerializeField] private InventoryData _inventoryData;

        [Header("PANEL")]
        [SerializeField] private GameObject _panelUpgradeSystem;
        [SerializeField] private GameObject _inventoryPanel;
        [SerializeField] private GameObject _panelUpgradeRequiriment;
        [SerializeField] private GameObject _panelLabelConfirm;
        [SerializeField] private GameObject _panelMaxLevel;
        [SerializeField] private GameObject[] _listOtherPanel;
        [SerializeField] private Volume _blurEffect;

        [Header("UI COMPONENT")]
        [SerializeField] private Sprite _frameItemDefault;
        [SerializeField] private Sprite _frameItemHover;
        [SerializeField] private TextMeshProUGUI _textEquipmentName1;
        [SerializeField] private TextMeshProUGUI _textEquipmentName2;

        [SerializeField] private int _activeIndex;
        private int _previousSelected;
        private int _size;

        [Header("Event")]
        [SerializeField] private VoidEventChannelSO _onEventUpgrade;

        private void OnEnable()
        {
            _onEventUpgrade.OnEventRaised += OpenBlacksmith;
        }

        private void OnDisable()
        {
            _onEventUpgrade.OnEventRaised -= OpenBlacksmith;
        }

        public void Start()
        {
            _panelUpgradeSystem.SetActive(true);
            PanelUpgrade[] itemPanelsInGrid = _inventoryPanel.GetComponentsInChildren<PanelUpgrade>();

            foreach (PanelUpgrade panel in itemPanelsInGrid)
            {
                _panelInventory.Add(panel);
                panel.gameObject.SetActive(false);
            }

            _upgradeEquipment = GetComponentInChildren<UIUpgradeEquipment>();
            _upgradeEquipment.Initialize(this, _inventoryData);

            _size = _inventoryData.inventoryIndex;
            _previousSelected = _size;

            _panelUpgradeSystem.SetActive(false);
            _panelUpgradeRequiriment.SetActive(false);
        }

        public void OpenBlacksmith()
        {
            ShowPanel();
            HideOtherUI();
            _panelLabelConfirm.SetActive(false);
            _panelUpgradeSystem.SetActive(true);
        }

        public void ShowPanel()
        {
            int amountEquipment = _inventoryData.inventorySize - _inventoryData.inventoryIndex;
            for (int i = 0; i < amountEquipment; i++)
            {
                _panelInventory[i].Initialize(this, i, _inventoryData.items[_inventoryData.inventoryIndex + i], CheckEquipmentLevel((EquipmentItem)_inventoryData.items[_size + i].item),  _frameItemDefault, _frameItemHover);
                _panelInventory[i].gameObject.SetActive(true);
            }
        }

        public void ReShowPanel()
        {
            ShowPanel();
            SelectedPanels(_activeIndex);
        }

        public void CloseBlacksmith()
        {
            _panelLabelConfirm.SetActive(false);
            _panelUpgradeSystem.SetActive(false);
            _panelUpgradeRequiriment.SetActive(false);
            ShowOtherUI();
        }

        public void SelectedPanels(int index)
        {
            _previousSelected = _activeIndex;
            _activeIndex = index;
            _panelInventory[_previousSelected].CancelSelected();
            
            EquipmentItem item = (EquipmentItem)_inventoryData.items[_size + _activeIndex].item;

            if (item != null)
            {
                if (CheckEquipmentLevel(item))
                {
                    _panelMaxLevel.SetActive(true);
                    _panelUpgradeRequiriment.SetActive(false);
                    _upgradeEquipment.ShowRequiriment(item, true);
                    _panelLabelConfirm.SetActive(false);
                    _textEquipmentName2.text = item.itemName;
                }
                else
                {
                    _upgradeEquipment.ShowRequiriment(item, false);
                    _panelMaxLevel.SetActive(false);
                    _panelUpgradeRequiriment.SetActive(true);
                    _panelLabelConfirm.SetActive(true);
                    _panelInventory[_activeIndex].HasSelected();
                    _textEquipmentName1.text = item.itemName;
                    Debug.Log("Not Null");
                }
            }
            else
            {
                Debug.Log("Null");
            }
        }

        public bool CheckEquipmentLevel(EquipmentItem item)
        {
            if (item.equipmentLevel >= item.upgradeRequiriment.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ShowOtherUI()
        {
            foreach (GameObject panel in _listOtherPanel)
            {
                panel.SetActive(true);
            }

            _blurEffect.enabled = false;
        }

        public void HideOtherUI()
        {
            foreach (GameObject panel in _listOtherPanel)
            {
                panel.SetActive(false);
            }

            _blurEffect.enabled = true;
        }

    }
}
