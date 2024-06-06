using CC.Inventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CC.Shop
{
    public class PanelInventorySell : MonoBehaviour, IPointerClickHandler
    {
        private SellManager _sellManager;
        private int _indexItem;
        private ItemSlotInfo _itemSlot;

        [Header("UI Component")]
        [SerializeField] private Image _frameItem;
        [SerializeField] private Image _imageItem;
        [SerializeField] private TextMeshProUGUI _textAmount;
        [SerializeField] private GameObject _equipIcon;

        private Sprite _frameDefault;
        private Sprite _frameHover;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_itemSlot == null) return;

            OnActionClick();
        }

        private void OnActionClick()
        {
            HasSelected();

            _sellManager.SwapSelected(_indexItem);
        }

        public void Initialize(SellManager sellManager, int index, ItemSlotInfo item, Sprite frameDefault, Sprite frameHover)
        {
            _sellManager = sellManager;
            _indexItem = index;
            _itemSlot = item;
            _frameDefault = frameDefault;
            _frameHover = frameHover;

            if(item == null)
            {
                HideUI();
            }
            else
            {
                SetUI();
            }

        }

        public void SetUI()
        {
            _imageItem.enabled = true;
            _textAmount.enabled = true;
            _imageItem.sprite = _itemSlot.item.itemSprite;
            _textAmount.text = _itemSlot.stacks.ToString();
        }

        private void HideUI()
        {
            _imageItem.enabled = false;
            _textAmount.enabled = false;
        }

        public void ShowEquipIcon()
        {
            _equipIcon.SetActive(true);
        }

        public void HideEquipIcon()
        {
            _equipIcon.SetActive(false);
        }

        public void HasSelected()
        {
            _frameItem.sprite = _frameHover;
        }

        public void CancelSelected()
        {
            _frameItem.sprite = _frameDefault;
        }


    }
}
