using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace CC.Quest.UI
{
    public class ItemRewardUIHandler : MonoBehaviour
    {
        [Header("Visuals")]
        [SerializeField] Image _icon;
        [SerializeField] TextMeshProUGUI _quantity;

        public void set(Sprite _img, int _quant)
        {
            _icon.sprite = _img;
            _quantity.text = _quant.ToString();
        }
    }
}
