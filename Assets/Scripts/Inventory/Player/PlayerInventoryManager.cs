using CC.Event;
using CC.Events;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;
using static UnityEditor.Timeline.Actions.MenuPriority;

namespace CC.Inventory
{
    public class PlayerInventoryManager : MonoBehaviour, IInventoryManager
    {
        [SerializeField] private AInventoryData _aitemInventoryData;
        private InventoryData _inventoryData;
        public List<AItemPanel> existingPanels = new List<AItemPanel>();
        private IInventoryManagement _playerInventoryManagement;
        private IInventoryAction _playerInventoryAction;
        private IInventoryWeight _playerInventoryWeight;

        [Space]
        [Header("Inventory Menu Components")]
        public GameObject inventoryMenuUI;
        public GameObject[] itemPanelGrid;
        [SerializeField] private ItemSlotMouse _itemSlotMouse;
        [HideInInspector] public bool isActived = false;

        [Header("Weight System")]
        [SerializeField] private TextMeshProUGUI _textWeight;
        private float _weightValue;
        [SerializeField] private InventoryDataEventChannel _shopEvent;
        [SerializeField] private InventoryDataEventChannel _unShopEvent;

        private void OnEnable()
        {
            _inventoryData.addItemToInventory.OnEventRaised += _playerInventoryManagement.OnAddItem;
            _inventoryData.removeItemEvent.OnEventRaised.AddListener(_playerInventoryManagement.OnRemoveItem);
            _inventoryData.itemCheckEvent.OnEventRaised += _playerInventoryAction.CheckItem;
            _inventoryData.onItemPickup.OnEventRaised += WeightCount;
            _inventoryData.onSellItem.OnEventRaised += _playerInventoryManagement.OnSellItem;
        }

        private void OnDisable()
        {
            _inventoryData.addItemToInventory.OnEventRaised -= _playerInventoryManagement.OnAddItem;
            _inventoryData.removeItemEvent.OnEventRaised.AddListener(_playerInventoryManagement.OnRemoveItem);
            _inventoryData.itemCheckEvent.OnEventRaised -= _playerInventoryAction.CheckItem;
            _inventoryData.onItemPickup.OnEventRaised -= WeightCount;
            _inventoryData.onSellItem.OnEventRaised -= _playerInventoryManagement.OnSellItem;
        }

        private void Awake()
        {
            _inventoryData = (InventoryData)_aitemInventoryData;

            _playerInventoryWeight = new PlayerInventoryWeight();
            _playerInventoryWeight.Initialize(_inventoryData);

            _playerInventoryManagement = new PlayerInventoryManagement();
            _playerInventoryManagement.Initialize(_inventoryData, this, _itemSlotMouse);

            _playerInventoryAction = new PlayerInventoryAction();
            _playerInventoryAction.Initialize(_inventoryData, this, _itemSlotMouse);
        }

        public void OpenInventory()
        {
            inventoryMenuUI.SetActive(true);

            Cursor.lockState = CursorLockMode.Confined;
            RefreshInventory();
            _inventoryData.inputReader.EnableInventoryUIInput();

            ShowShopkeeper();
        }

        public void CloseInventory()
        {
            inventoryMenuUI.SetActive(false);

            _itemSlotMouse.EmptySlot();
            Cursor.lockState = CursorLockMode.Confined;
            _inventoryData.inputReader.EnableGameplayInput();

            UnShowShopkeeper();
        }

        public void ShowShopkeeper()
        {
            _shopEvent.RaiseEvent(_aitemInventoryData);
            _inventoryData.inputReader.DropItemPerformed -= _playerInventoryAction.OnDropItem;
        }

        public void UnShowShopkeeper()
        {
            _unShopEvent.RaiseEvent(_aitemInventoryData);
            _inventoryData.inputReader.DropItemPerformed += _playerInventoryAction.OnDropItem;
        }

        private void WeightCount()
        {
            _weightValue = _playerInventoryWeight.GetWeight();
            _textWeight.text = "Weight : " + _weightValue.ToString();
        }

        public void ClearSlot(ItemSlotInfo slot)
        {
            slot.item = null;
            slot.stacks = 0;
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

                        if (i.stacks > 1)
                        {
                            panel.stacksText.text = "" + i.stacks;
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
