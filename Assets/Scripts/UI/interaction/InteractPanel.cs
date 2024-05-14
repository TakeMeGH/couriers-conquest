using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace CC
{
    public class InteractPanel : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _interactionText;
        [SerializeField] Image _interactionIcon;
        [SerializeField] Image _interactionSelected;
        Sprite _icon;
        int _amount;
        string _interactionName;

        public void Init(Sprite _icon, int _amount, string _interactionName)
        {
            this._icon = _icon;
            this._amount = _amount;
            this._interactionName = _interactionName;

            _interactionText.text = _interactionName;
            _interactionIcon.sprite = _icon;
        }

        public void EnableSelected()
        {
            _interactionSelected.gameObject.SetActive(true);
        }

        public void DisableSelected()
        {
            _interactionSelected.gameObject.SetActive(false);
        }

        public bool IsSelected()
        {
            return _interactionSelected.gameObject.activeInHierarchy;

        }

        // public bool IsMatch(Sprite _targetIcon, int _targetAmount, string _targetInteractionName)
        // {
        //     if (_icon != _targetIcon)
        //     {
        //         return false;
        //     }

        //     if (_amount != _targetAmount)
        //     {
        //         return false;
        //     }

        //     if (_interactionName != _targetInteractionName)
        //     {
        //         return false;
        //     }
        //     return true;
        // }
    }
}
