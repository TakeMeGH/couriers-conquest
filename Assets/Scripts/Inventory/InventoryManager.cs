using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using CC.Events;
using TMPro;
using UnityEngine.InputSystem;
using System;

namespace CC.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeReference] public List<ItemSlotInfo> items = new List<ItemSlotInfo>();

        [Space]
        [Header("Inventory Menu Components")]
        [SerializeField] private GameObject _inventoryMenu;
        [SerializeField] private GameObject[] _itemPanelGrid;
        [SerializeField] private Mouse _itemSlotMouse;
        public Mouse mouse { get => _itemSlotMouse; }

        [SerializeField] private List<ItemPanel> _existingPanels = new List<ItemPanel>();

        [Space]
        [SerializeField] private int _inventorySize = 24;
        [SerializeField] private float _dropSpeed = 5;
        [SerializeField] InputReader _inputReader;
        [SerializeField] ItemInventoryEventChannel _addItemToInventory;

        [Header("Weight System")]
        [SerializeField] private TextMeshProUGUI _textWeight;
        [SerializeField] FloatEventChannelSO _onWeightUpdated;
        [SerializeField] VoidEventChannelSO _onItemPickup;

        private float _weightValue;
        private ItemDictionary _itemDictionary;

        private void OnEnable()
        {
            _inputReader.DropItemPerformed += AttemptToDrop;
            _addItemToInventory.OnEventRaised += AddItem;
            _onItemPickup.OnEventRaised += WeightCount;
        }

        private void OnDisable()
        {
            _inputReader.DropItemPerformed -= AttemptToDrop;
            _addItemToInventory.OnEventRaised -= AddItem;
            _onItemPickup.OnEventRaised -= WeightCount;
        }

        private void Awake()
        {
            _itemDictionary = new ItemDictionary();
            _itemDictionary.Initialize();
        }

        private void Start()
        {
            Initialize();
            SetDefaultEquipment();
        }
        public void OpenInventory()
        {
            _inventoryMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            RefreshInventory();
            _inputReader.EnableInventoryUIInput();

        }

        public void CloseInventory()
        {
            _inventoryMenu.SetActive(false);
            _itemSlotMouse.EmptySlot();
            Cursor.lockState = CursorLockMode.Locked;
            _inputReader.EnableGameplayInput();
        }

        private void AttemptToDrop()
        {
            if (_itemSlotMouse.itemSlot.item != null && !EventSystem.current.IsPointerOverGameObject())
            {
                DropItem(_itemSlotMouse.itemSlot.item);
            }
        }

        private void Initialize()
        {
            for(int i = 0; i < _itemPanelGrid.Length; i++)
            {
                ItemPanel[] itemPanelsInGrid = _itemPanelGrid[i].GetComponentsInChildren<ItemPanel>();
                _existingPanels.AddRange(itemPanelsInGrid);
            }

            for (int i = 0; i < _existingPanels.Count; i++)
            {
                items.Add(new ItemSlotInfo(null, 0));
                _existingPanels[i].mouse = mouse;
            }
        }

        private void SetDefaultEquipment()
        {
            AddItem(_itemDictionary.GetValueByKey("Long Sword"), 1);
            AddItem(_itemDictionary.GetValueByKey("Basic Armor"), 1);
            AddItem(_itemDictionary.GetValueByKey("Basic Shield"), 1);

            for (int i = _inventorySize; i < _existingPanels.Count; i++)
            {
                AttachDefaultItem(i);
                Debug.Log("Try " + i.ToString());
            }
;       }

        private void AttachDefaultItem(int targetSlot)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].item == null) continue;

                if (items[i].item.GetItemType() == ItemType.Equipment)
                {
                    if(((EquipmentItem)items[i].item).specificType == _existingPanels[targetSlot].slotType)
                    {
                        items[targetSlot].item = items[i].item;
                        items[targetSlot].stacks = items[i].stacks; 
                        items[i].item = null;
                        break;
                    }
                    else
                    {
                        Debug.Log(targetSlot.ToString() + " Not Added");
                    }
                }
            }

            RefreshInventory();
        }

        public void RefreshInventory()
        {
            int index = 0;
            foreach (ItemSlotInfo i in items)
            {
                //Name our List Elements
                i.name = "" + (index + 1);
                if (i.item != null) i.name += ": " + i.item.itemName;
                else i.name += ": -";

                //Update our Panels
                ItemPanel panel = _existingPanels[index];
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

                        if(i.stacks > 1)
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

        private void WeightCount()
        {
            _textWeight.text = "Weight : " + WeightValue().ToString();
        }

        public float WeightValue()
        {
            _weightValue = 0;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].item != null)
                {
                    _weightValue += items[i].item.itemWeight * items[i].stacks;
                }
            }
            _onWeightUpdated.RaiseEvent(_weightValue);
            return _weightValue;
        }

        public int AddItem(ABaseItem item, int amount)
        {

            if (item == null)
            {
                Debug.Log("Could not find Item in Dictionary to add to Inventory");
                return amount;
            }

            //Check for open spaces in existing slots
            foreach (ItemSlotInfo i in items)
            {
                if (i.item != null)
                {
                    if (i.item.itemName == item.itemName)
                    {
                        if (amount > i.item.maxStacks - i.stacks)
                        {
                            amount -= i.item.maxStacks - i.stacks;
                            i.stacks = i.item.maxStacks;
                        }
                        else
                        {
                            i.stacks += amount;
                            if (_inventoryMenu.activeSelf) RefreshInventory();
                            return 0;
                        }
                    }
                }
            }
            //Fill empty slots with leftover items
            foreach (ItemSlotInfo i in items)
            {
                if (i.item == null)
                {
                    if (amount > item.maxStacks)
                    {
                        i.item = item;
                        i.stacks = item.maxStacks;
                        amount -= item.maxStacks;
                    }
                    else
                    {
                        i.item = item;
                        i.stacks = amount;
                        if (_inventoryMenu.activeSelf) RefreshInventory();
                        return 0;
                    }
                }
            }
            //No space in Inventory, return remainder items
            Debug.Log("No space in Inventory for: " + item.itemName);
            if (_inventoryMenu.activeSelf) RefreshInventory();
            return amount;
        }

        public void DropItem(ABaseItem item)
        {
            if (item == null)
            {
                Debug.Log("Could not find Item in Dictionary to add to drop");
                return;
            }

            Transform camTransform = UnityEngine.Camera.main.transform;
            Transform _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            GameObject dropPrefab = ObjectPooling.SharedInstance.GetPooledObject(PoolObjectType.Item);
            if (dropPrefab != null)
            {
                dropPrefab.transform.position = _playerTransform.position + new Vector3(0, 1.8f, 0) + camTransform.forward;
                dropPrefab.transform.rotation = _playerTransform.rotation;
                dropPrefab.SetActive(true);
            }

            Rigidbody rb = dropPrefab.GetComponent<Rigidbody>();
            if (rb != null) rb.velocity = camTransform.forward * _dropSpeed;

            ItemPickup ip = dropPrefab.GetComponentInChildren<ItemPickup>();
            ip.isDropItem = true;
            if (ip != null)
            {
                ip.item = item;
                ip.amount = _itemSlotMouse.splitSize;
                _itemSlotMouse.itemSlot.stacks -= _itemSlotMouse.splitSize;
            }

            if (_itemSlotMouse.itemSlot.stacks < 1) ClearSlot(_itemSlotMouse.itemSlot);
            _itemSlotMouse.EmptySlot();
            RefreshInventory();
        }

        public void UseItem(ABaseItem item)
        {
            item.UseItem();
        }

        public void ClearSlot(ItemSlotInfo slot)
        {
            slot.item = null;
            slot.stacks = 0;
        }
    }

    public enum ItemSlotType{
        Inventory,
        Weapon,
        Shield,
        Armor,
        Consumable
    }
}

