using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CC.Inventory
{
    public class ItemSlotMouse : MonoBehaviour
    {
        public GameObject mouseItemUI;
        public Image mouseCursor;
        public ItemSlotInfo itemSlot;
        public ItemSlotType slotType;

        [SerializeField] private List<Color> _backgroundColor;

        public AItemPanel sourceItemPanel;
        public int splitSize;

        [Header("UI COMPONENT")]
        [SerializeField] private Image _itemImage;
        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private TextMeshProUGUI _itemWeight;
        [SerializeField] private TextMeshProUGUI _itemEffect;
        [SerializeField] private TextMeshProUGUI _itemDescription;
        [SerializeField] private Image _background;

        private void Start()
        {
            // Cursor.lockState = CursorLockMode.Locked;

        }
        void Update()
        {
            /*transform.position = Input.mousePosition;
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                mouseCursor.enabled = false;
                mouseItemUI.SetActive(false);
            }
            else
            {
                mouseCursor.enabled = true;

                if (itemSlot.item != null)
                {
                    mouseItemUI.SetActive(true);
                }
                else
                {
                    mouseItemUI.SetActive(false);
                }
            }
            if (itemSlot.item != null)
            {
                if (Input.GetKey(KeyCode.Mouse1))
                {
                    sourceItemPanel.RefreshInventory();
                    return;
                }

                if (Input.GetAxis("Mouse ScrollWheel") > 0 && splitSize < itemSlot.stacks)
                {
                    splitSize++;
                }
                if (Input.GetAxis("Mouse ScrollWheel") < 0 && splitSize > 1)
                {
                    splitSize--;
                }

                stacksText.text = "" + splitSize;



                if (splitSize == itemSlot.stacks && sourceItemPanel.stacksText?.gameObject != null)
                {
                    sourceItemPanel.stacksText.gameObject.SetActive(false);
                }
                else
                {
                    if (sourceItemPanel.stacksText != null)
                    {
                        sourceItemPanel.stacksText.gameObject.SetActive(true);
                        sourceItemPanel.stacksText.text = "" + (itemSlot.stacks - splitSize);
                    }
                    else
                    {
                        return;
                    }
                    
                }
            }*/
        }

        public void SetUI()
        {
            _itemName.text = itemSlot.item.name;
            _itemWeight.text = (itemSlot.item.itemWeight * itemSlot.stacks).ToString();
            _itemDescription.text = itemSlot.item.itemDescription;

            _itemImage.sprite = itemSlot.item.itemSprite;

            SetItemType();
        }

        private void SetItemType()
        {
            if (itemSlot.item.GetItemType() == ItemType.Materials)
            {
                _background.color = _backgroundColor[0];
            }
            else if(itemSlot.item.GetItemType() == ItemType.QuestItem)
            {
                _background.color = _backgroundColor[1];
            }
            else if (itemSlot.item.GetItemType() == ItemType.Consumable)
            {
                _background.color = _backgroundColor[2];
            }
            else if (itemSlot.item.GetItemType() == ItemType.DropMonster)
            {
                _background.color = _backgroundColor[3];
            }
        }

        public void EmptySlot()
        {
            itemSlot = new ItemSlotInfo(null, 0);
        }
    }
}