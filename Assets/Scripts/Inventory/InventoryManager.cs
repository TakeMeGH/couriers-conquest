using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using CC.Events;

namespace CC.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeReference] public List<ItemSlotInfo> items = new List<ItemSlotInfo>();

        [Space]
        [Header("Inventory Menu Components")]
        [SerializeField] private GameObject _inventoryMenu;
        [SerializeField] private GameObject _itemPanel;
        [SerializeField] private GameObject _itemPanelGrid;
        [SerializeField] private Mouse _mouse;
        public Mouse mouse { get => _mouse; }

        private List<ItemPanel> existingPanels = new List<ItemPanel>();

        [Space]
        [SerializeField] private int _inventorySize = 24;
        [SerializeField] private float _dropSpeed = 5;
        [SerializeField] private Volume _blurEffect;
        [SerializeField] InputReader _inputReader;
        [SerializeField] ItemInventoryEventChannel _addItemToInventory;

        private void OnEnable()
        {
            _inputReader.OpenInventoryEvent += OpenInventory;
            _inputReader.CloseInventoryEvent += CloseInventory;
            _inputReader.DropItemPerformed += AttemptToDrop;
            _addItemToInventory.OnEventRaised += AddItem;
        }

        private void OnDisable()
        {
            _inputReader.OpenInventoryEvent -= OpenInventory;
            _inputReader.CloseInventoryEvent -= CloseInventory;
            _inputReader.DropItemPerformed -= AttemptToDrop;
            _addItemToInventory.OnEventRaised += AddItem;



        }
        void Start()
        {
            for (int i = 0; i < _inventorySize; i++)
            {
                items.Add(new ItemSlotInfo(null, 0));
            }
        }
        void OpenInventory()
        {
            _inventoryMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            _blurEffect.enabled = true;
            RefreshInventory();
            _inputReader.EnableInventoryUIInput();

        }

        void CloseInventory()
        {
            _inventoryMenu.SetActive(false);
            _mouse.EmptySlot();
            _blurEffect.enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
            _inputReader.EnableGameplayInput();


        }

        void AttemptToDrop()
        {
            if (_mouse.itemSlot.item != null && !EventSystem.current.IsPointerOverGameObject())
            {
                DropItem(_mouse.itemSlot.item);

            }
        }

        public void RefreshInventory()
        {
            existingPanels = _itemPanelGrid.GetComponentsInChildren<ItemPanel>().ToList();
            //Create Panels if needed
            if (existingPanels.Count < _inventorySize)
            {
                int amountToCreate = _inventorySize - existingPanels.Count;
                for (int i = 0; i < amountToCreate; i++)
                {
                    GameObject newPanel = Instantiate(_itemPanel, _itemPanelGrid.transform);
                    existingPanels.Add(newPanel.GetComponent<ItemPanel>());
                }
            }

            int index = 0;
            foreach (ItemSlotInfo i in items)
            {
                //Name our List Elements
                i.name = "" + (index + 1);
                if (i.item != null) i.name += ": " + i.item.itemName;
                else i.name += ": -";

                //Update our Panels
                ItemPanel panel = existingPanels[index];
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
                        panel.stacksText.text = "" + i.stacks;
                    }
                    else
                    {
                        panel.itemImage.gameObject.SetActive(false);
                        panel.stacksText.gameObject.SetActive(false);
                    }
                }
                index++;
            }
            _mouse.EmptySlot();
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
            //Exit method if no Item was found
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
                ip.amount = _mouse.splitSize;
                _mouse.itemSlot.stacks -= _mouse.splitSize;
            }

            if (_mouse.itemSlot.stacks < 1) ClearSlot(_mouse.itemSlot);
            _mouse.EmptySlot();
            RefreshInventory();
        }

        public void ClearSlot(ItemSlotInfo slot)
        {
            slot.item = null;
            slot.stacks = 0;
        }
    }
}