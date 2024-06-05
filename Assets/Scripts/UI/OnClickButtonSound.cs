using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CC
{
    public class OnClickButtonSound : MonoBehaviour
    {
        [SerializeField] ClickSound _clickSound;
        Button _button;

        private void OnEnable()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClickSound);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClickSound);
        }

        void OnClickSound()
        {
            switch (_clickSound)
            {
                case ClickSound.Confirm:
                    AudioManager.instance.AudioPlayOneShot(AudioManager.instance.ConfirmUI, transform.position);
                    break;
                case ClickSound.Back:
                    AudioManager.instance.AudioPlayOneShot(AudioManager.instance.BackUI, transform.position);
                    break;
                case ClickSound.Dialogue:
                    AudioManager.instance.AudioPlayOneShot(AudioManager.instance.DialogueUI, transform.position);
                    break;

            }
        }
        private void Update()
        {
            // Debug.Log();
            // _button.
        }
    }

    public enum ClickSound
    {
        Confirm,
        Back,
        Dialogue
    }
}
