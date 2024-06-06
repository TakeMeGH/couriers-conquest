using CC.Inventory;
using CC.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CC.UpgradeEquipment
{
    public class PanelMaterialsUpgrade : MonoBehaviour
    {

        [SerializeField] private ABaseItem _items;

        [Space][Header("UI COMPONENT")]
        private UIUpgradeEquipment _upgradeEquipment;
        [SerializeField] private Image _frameItem;
        [SerializeField] private Image _itemImage;
        [SerializeField] private TextMeshProUGUI _textRequiriment;
        [SerializeField] private Sprite _spriteDefault;
        [SerializeField] private Sprite _spriteHover;

        private int _amountNow;
        private int _amountRequiriment;
        private bool _isAllready;

        public bool isAllready { get => _isAllready; }

        public void Initialize(UIUpgradeEquipment upgradeEquipment, ABaseItem items, int amountNow, int amountRequiriment)
        {
            _upgradeEquipment = upgradeEquipment;
            _items = items;
            _amountNow = amountNow;
            _amountRequiriment = amountRequiriment;
            SetUI();
        }

        private void SetUI()
        {
            _itemImage.sprite = _items.itemSprite;
            _textRequiriment.text = _amountNow.ToString() + "/" + _amountRequiriment.ToString();
            _isAllready = CheckRequiriment();
        }

        private bool CheckRequiriment()
        {
            if (_amountNow >= _amountRequiriment)
            {
                _frameItem.sprite = _spriteHover;
                _textRequiriment.color = Color.white;
                SetTranparant(1);
                return true;
            }
            else
            {
                _frameItem.sprite = _spriteDefault;
                _textRequiriment.color = Color.red;
                SetTranparant(0.5f);
                return false;
            }
        }

        public void SetTranparant(float alpha)
        {
            if (_itemImage != null)
            {
                Color color = _itemImage.color;
                color.a = alpha;
                _itemImage.color = color;
            }
        }
    }
}
