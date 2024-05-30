using CC.Event;
using CC.Events;
using CC.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CC.Inventory
{
    public class PlayerInventoryManager : MonoBehaviour, IInventoryManager
    {
        [SerializeField] private AInventoryData _aitemInventoryData;
        //[SerializeField] private UIPlayerStatus _uiPlayerStatus;
        private PouchAndRuneManager _pouchRuneManager;
        public GameObject itemDetailPanel;
        [SerializeField] private GameObject _hudPanel;

        private InventoryData _inventoryData;
        public List<PanelInventory> existingPanels = new List<PanelInventory>();
        public ABaseItem[] _defaultEquipment;

        private IInventoryManagement _playerInventoryManagement;
        private PlayerInventoryAction _playerInventoryAction;
        private IInventoryWeight _playerInventoryWeight;

        public FMOD.Studio.EventInstance openSound;

        [Space]
        [Header("Inventory Menu Components")]
        public GameObject inventoryMenuUI;
        public GameObject[] itemPanelGrid;
        [SerializeField] private ItemSlotMouse _itemSlotMouse;
        [HideInInspector] public bool isActived = false;

        [Header("Weight System")]
        [SerializeField] private TextMeshProUGUI _textWeight;
        private float _weightValue;

        [Header("Equiped")]
        [SerializeField] public ItemType activeSlot;
        [SerializeField] private GameObject _labelDropSlotItem;
        [SerializeField] private GameObject _labelEquipSlotItem;
        [SerializeField] private GameObject _labelConsumeSlotItem;
        [SerializeField] private TextMeshProUGUI _textLabelEquiped;
        [SerializeField] private Sprite _frameItemDefault;
        [SerializeField] private Sprite _frameItemHover;
        public int activeIndexItemSlot;
        private int _previousActive;
        public InventoryActionType actionType = InventoryActionType.None;

        [Header("Drop Item")]
        [SerializeField] private float maxHoldDuration = 3f;
        public bool isHoldDrop;
        [HideInInspector] public bool canDrop;
        public Slider sliderDrop;

        private void OnEnable()
        {
            _inventoryData = (InventoryData)_aitemInventoryData;
            _playerInventoryWeight = new PlayerInventoryWeight();
            _playerInventoryManagement = new PlayerInventoryManagement();
            _playerInventoryAction = new PlayerInventoryAction();
            _pouchRuneManager = GetComponent<PouchAndRuneManager>();
            Initalize();

            _inventoryData.addItemToInventory.OnEventRaised += _playerInventoryManagement.OnAddItem;
            _inventoryData.removeItemEvent.OnEventRaised.AddListener(_playerInventoryManagement.OnRemoveItem);
            _inventoryData.itemCheckEvent.OnEventRaised += _playerInventoryAction.CheckItem;
            _inventoryData.onItemPickup.OnEventRaised += WeightCount;
            _inventoryData.onSellItem.OnEventRaised += _playerInventoryManagement.OnSellItem;
            _inventoryData.onUpdateCurrency.OnEventRaised += _playerInventoryManagement.OnUpdateCurrency;
            _inventoryData.onUpgradeEquipment.OnEventRaised += _playerInventoryAction.UpgradeItem;

            _inventoryData.inputReader.EquipItemPerformed += OnEquipSlot;
            _inventoryData.inputReader.DropItemPerformed += _playerInventoryAction.DropPerformed;
            _inventoryData.inputReader.DropItemCanceled += _playerInventoryAction.DropCanceled;
            _inventoryData.inputReader.ConsumeItemPerformed += OnConsumeItem;

            //_uiPlayerStatus.Initialize(this, _inventoryData);
        }

        private void OnDisable()
        {
            _inventoryData.addItemToInventory.OnEventRaised -= _playerInventoryManagement.OnAddItem;
            _inventoryData.removeItemEvent.OnEventRaised.AddListener(_playerInventoryManagement.OnRemoveItem);
            _inventoryData.itemCheckEvent.OnEventRaised -= _playerInventoryAction.CheckItem;
            _inventoryData.onItemPickup.OnEventRaised -= WeightCount;
            _inventoryData.onSellItem.OnEventRaised -= _playerInventoryManagement.OnSellItem;
            _inventoryData.onUpdateCurrency.OnEventRaised -= _playerInventoryManagement.OnUpdateCurrency;
            _inventoryData.onUpgradeEquipment.OnEventRaised -= _playerInventoryAction.UpgradeItem;

            _inventoryData.inputReader.EquipItemPerformed -= OnEquipSlot;
            _inventoryData.inputReader.DropItemPerformed -= _playerInventoryAction.DropPerformed;
            _inventoryData.inputReader.DropItemCanceled -= _playerInventoryAction.DropCanceled;
            _inventoryData.inputReader.ConsumeItemPerformed -= OnConsumeItem;
        }

        public void Initalize()
        {
            _playerInventoryWeight.Initialize(_inventoryData);
            _playerInventoryManagement.Initialize(_inventoryData, this, _itemSlotMouse);
            _playerInventoryAction.Initialize(_inventoryData, this, _itemSlotMouse, sliderDrop);
            _pouchRuneManager.Initialize(this, _inventoryData);

            inventoryMenuUI.SetActive(true);
            inventoryMenuUI.SetActive(false);
        }

        private void Update()
        {
            if (isHoldDrop)
            {
                _playerInventoryAction.HoldAttemp();
            }
        }

        public void OpenInventory()
        {
            inventoryMenuUI.SetActive(true);

            RefreshInventory();
            _inventoryData.inputReader.EnableInventoryUIInput();
            //_uiPlayerStatus.HidePouchPanel();
            itemDetailPanel.SetActive(false);
            sliderDrop.gameObject.SetActive(false);
            activeSlot = ItemType.None;
            existingPanels[activeIndexItemSlot].ChangeFrameSlotUI(_frameItemDefault);

            FMODUnity.RuntimeManager.PlayOneShotAttached("event:/SFX/Character/Walk", gameObject);
            _hudPanel.SetActive(false);
        }

        public void CloseInventory()
        {
            inventoryMenuUI.SetActive(false);
            _itemSlotMouse.EmptySlot();
            _inventoryData.inputReader.EnableGameplayInput();
            _hudPanel.SetActive(true);
        }

        public void SwapActiveSlot(int targetActive)
        {
            _previousActive = activeIndexItemSlot;
            existingPanels[_previousActive].ChangeFrameSlotUI(_frameItemDefault);

            activeIndexItemSlot = targetActive;
            existingPanels[activeIndexItemSlot].ChangeFrameSlotUI(_frameItemHover);
        }

        public void ChangeFrameToDefault(int index)
        {
            existingPanels[index].ForceChangeFrame(_frameItemDefault);
        }

        public void RefreshPanelItem()
        {
            existingPanels[activeIndexItemSlot].isNull = true;
            existingPanels[activeIndexItemSlot].ChangeFrameSlotUI(_frameItemDefault);
        }

        private void WeightCount()
        {
            _weightValue = _playerInventoryWeight.GetWeight();
            _textWeight.text = _weightValue.ToString();
        }

        public void ClearSlot(ItemSlotInfo slot)
        {
            slot.item = null;
            slot.stacks = 0;
        }

        public void SetLabelConsumableType()
        {
            _labelConsumeSlotItem.SetActive(true);
            _labelDropSlotItem.SetActive(true);
        }

        public void SetLabelMaterialsType()
        {
            _labelDropSlotItem.SetActive(true);
            _labelConsumeSlotItem.SetActive(false);
            _labelEquipSlotItem.SetActive(false);
        }

        public void SetNoActionLabel()
        {
            _labelDropSlotItem.SetActive(false);
            _labelConsumeSlotItem.SetActive(false);
            _labelEquipSlotItem.SetActive(false);
        }

        public void CanEquipSpesifikSlot()
        {
            _labelEquipSlotItem.SetActive(true);
            _labelConsumeSlotItem.SetActive(false);

            if ((_inventoryData.isPouchEquiped && activeIndexItemSlot == _inventoryData.indexPouchEquiped) || (_inventoryData.isRuneEquiped && activeIndexItemSlot == _inventoryData.indexRuneEquiped))
            {
                _textLabelEquiped.text = "Unequip Item";
                actionType = InventoryActionType.Unequip;
            }
            else if((!_inventoryData.isRuneEquiped) || (!_inventoryData.isPouchEquiped))
            {
                _textLabelEquiped.text = "Equip Item";
                actionType = InventoryActionType.Equip;
            }
            else
            {
                actionType = InventoryActionType.None;
            }
        }

        private void OnEquipSlot()
        {
            if(actionType == InventoryActionType.None) return;

            if(actionType == InventoryActionType.Equip)
            {
                if(activeSlot == ItemType.Consumable)
                {
                    _pouchRuneManager.EquipPouch(existingPanels[activeIndexItemSlot].itemSlot.item, activeIndexItemSlot);
                    existingPanels[activeIndexItemSlot].OnAction();
                }else if(activeSlot == ItemType.Rune)
                {
                    _pouchRuneManager.EquipRune(existingPanels[activeIndexItemSlot].itemSlot.item, activeIndexItemSlot);
                    existingPanels[activeIndexItemSlot].OnAction();
                }
            }else if(actionType == InventoryActionType.Unequip)
            {
                if (activeSlot == ItemType.Consumable)
                {
                    UnEquipSlot(ItemType.Consumable);
                }else if(activeSlot == ItemType.Rune)
                {
                    UnEquipSlot(ItemType.Rune);
                }
            }
        }

        public void UnEquipSlot(ItemType type)
        {
            if(type == ItemType.Consumable)
            {
                _pouchRuneManager.UnEquipPouch();
            }else if(type == ItemType.Rune)
            {
                _pouchRuneManager.UnEquipRune();
            }

            existingPanels[activeIndexItemSlot].ChangeFrameSlotUI(_frameItemDefault);
            itemDetailPanel.SetActive(false);
        }

        private void OnConsumeItem()
        {
            if(activeSlot == ItemType.Consumable)
            {
                _pouchRuneManager.AttempToConsumeItem(activeIndexItemSlot);
            }
        }

        public void RefreshInventory()
        {
            int index = 0;
            foreach (ItemSlotInfo i in _inventoryData.items)
            {
                //Name our List Elements
                i.name = "" + (index + 1);
                if (i.item != null) i.name += ": " + i.item.itemName;
                else i.name += ": -";

                //Update our Panels
                AItemPanel panel = existingPanels[index];
                panel.name = i.name + " Panel";
                if (panel != null)
                {
                    panel.inventory = this;
                    panel.itemSlot = i;
                    if (i.item != null)
                    {
                        panel.itemImage.gameObject.SetActive(true);
                        panel.itemImage.sprite = i.item.itemSprite;
                        panel.itemImage.CrossFadeAlpha(1, 0.05f, true);
                        panel.stacksText.gameObject.SetActive(true);

                        if(i.stacks <= 0)
                        {
                            i.item = null;
                            i.stacks = 0;
                            panel.isNull = true;
                            panel.stacksText.gameObject.SetActive(false);
                            panel.itemImage.gameObject.SetActive(false);
                        }else if(i.stacks > 0)
                        {
                            if (i.stacks == 1)
                            {
                                panel.stacksText.text = "";
                            }
                            else
                            {
                                panel.stacksText.text = "" + i.stacks;
                            }

                            panel.isNull = false;
                        }
                        else
                        {
                            panel.stacksText.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        panel.itemImage.gameObject.SetActive(false);
                        panel.stacksText.gameObject.SetActive(false);
                    }
                }
                index++;
            }

            _itemSlotMouse.EmptySlot();
            WeightCount();
        }

    }
}
