using CC.Inventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CC.UpgradeEquipment
{
    public class PanelUpgrade : MonoBehaviour, IPointerClickHandler
    {
        private BlacksmithManager _blacksmithManager;
        private int _indexItem;
        private ItemSlotInfo _itemSlot;

        [Header("UI Component")]
        [SerializeField] private Image _frameItem;
        [SerializeField] private Image _imageItem;

        private Sprite _frameDefault;
        private Sprite _frameHover;
        private bool _isMaxLevel;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_itemSlot == null) return;

            OnActionClick();
        }

        private void OnActionClick()
        {
            _blacksmithManager.SelectedPanels(_indexItem);
        }

        public void Initialize(BlacksmithManager sellManager, int index, ItemSlotInfo item, bool isMax, Sprite frameDefault, Sprite frameHover)
        {
            _blacksmithManager = sellManager;
            _indexItem = index;
            _itemSlot = item;
            _frameDefault = frameDefault;
            _frameHover = frameHover;


            if(isMax)
            {
                SetTranparant(0.4f);
                _isMaxLevel = true;
            }
            else
            {
                SetTranparant(1f);
                _isMaxLevel = false;
            }

            SetUI();
        }

        public void SetUI()
        {
            _imageItem.enabled = true;
            _imageItem.sprite = _itemSlot.item.itemSprite;
        }

        public void HasSelected()
        {
            _frameItem.sprite = _frameHover;
        }

        public void CancelSelected()
        {
            _frameItem.sprite = _frameDefault;
        }

        public void SetTranparant(float alpha)
        {
            if (_imageItem != null)
            {
                Color color = _imageItem.color;
                color.a = alpha;
                _frameItem.color = color;
                _imageItem.color = color;
            }
        }
    }
}
