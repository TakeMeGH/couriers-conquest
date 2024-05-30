using CC.Inventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CC.Shop
{
    public class BuyItemPanel : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        private BuyManager _buyInventoryManager;
        private int _indexPanel;

        [Header("UI COMPONENT")]
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _textCost;
        [SerializeField] private TextMeshProUGUI _textName;

        [SerializeField] private Image _frameBackground;
        [SerializeField] private Image _frameItem;
        [SerializeField] private Image _frameCost;

        public void Initialize(BuyManager buyInventoryManager, int index)
        {
            _buyInventoryManager = buyInventoryManager;
            _indexPanel = index;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _buyInventoryManager.activePanelIndex = _indexPanel;
            _buyInventoryManager.DeselectAllBuyPanel();
            _buyInventoryManager.SetSelectedPanel();
            OnAction();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            eventData.pointerPress = this.gameObject;
        }

        public void OnAction()
        {
            _buyInventoryManager.SelectItem();
        }

        public void SetUI(Sprite icon, string name, string cost)
        {
            _iconImage.sprite = icon;
            _textName.text = name;
            _textCost.text = cost;
        }

        public void SetDeselectedPanel(Sprite frameBackground, Sprite frameItem, Sprite frameCost)
        {
            _frameBackground.sprite = frameBackground;
            _frameItem.sprite = frameItem;
            _frameCost.sprite = frameCost;
        }

        public void SetSelectedPanel(Sprite frameBackground, Sprite frameItem, Sprite frameCost)
        {
            _frameBackground.sprite = frameBackground;
            _frameItem.sprite = frameItem;
            _frameCost.sprite = frameCost;
        }
    }
}
