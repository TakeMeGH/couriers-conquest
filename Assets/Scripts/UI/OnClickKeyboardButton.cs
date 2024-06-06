using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CC
{
    public class OnClickKeyboardButton : MonoBehaviour
    {
        [SerializeField] ClickType _clickType;
        [SerializeField] InputReader _inputReader;

        Button _button;

        private void OnEnable()
        {
            _button = GetComponent<Button>();
            switch (_clickType)
            {
                case ClickType.Confirm:
                    _inputReader.ConfirmActionPerformed += OnClick;
                    break;
                case ClickType.Back:
                    _inputReader.BackActionPerformed += OnClick;
                    break;
            }
        }

        private void OnDisable()
        {
            _button = GetComponent<Button>();
            switch (_clickType)
            {
                case ClickType.Confirm:
                    _inputReader.ConfirmActionPerformed -= OnClick;
                    break;
                case ClickType.Back:
                    _inputReader.BackActionPerformed -= OnClick;
                    break;
            }

        }

        void OnClick()
        {
            _button.onClick.Invoke();
        }
    }
    public enum ClickType
    {
        Confirm,
        Back,
    }

}
