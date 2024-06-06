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
        [SerializeField] private GameObject _panelQuality;

        private void Start()
        {
            // Cursor.lockState = CursorLockMode.Locked;
        }
    
        public void SetUI()
        {
            if (itemSlot.item.GetItemType() == ItemType.QuestItem)
            {
                QuestItem item = (QuestItem)itemSlot.item;
                _panelQuality.SetActive(true);
                _itemEffect.text = ((int)item.CurrentQuality).ToString() + "%";
            }
            else
            {
                _panelQuality.SetActive(false);
            }

            _itemName.text = itemSlot.item.itemName;
            _itemWeight.text = " " + itemSlot.item.itemWeight.ToString() + " kg";
            _itemDescription.text =  itemSlot.item.itemDescription;

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