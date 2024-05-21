using UnityEngine;
using TMPro;
using System;

namespace CC
{
    public class FloatingDamageController : MonoBehaviour
    {
        [SerializeField] TMP_Text _text;
        [SerializeField] Animator _animator;
        [SerializeField] float _startDisappearTime;
        [SerializeField] float _disappearTime;
        float _currentTime;

        public void Init(string text)
        {
            _text.text = text;
            _animator.SetTrigger("Show");

            _text.color = new Color(_text.color.r,
                        _text.color.g,
                        _text.color.b,
                        1);

            _currentTime = 0;
        }

        private void Update()
        {
            if (_currentTime >= _startDisappearTime)
            {
                if (_currentTime <= _startDisappearTime + _disappearTime)
                {
                    _currentTime += Time.deltaTime;
                    _text.color = new Color(_text.color.r,
                        _text.color.g,
                        _text.color.b,
                        Mathf.Lerp(1, 0, (_currentTime - _startDisappearTime) / _disappearTime));
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else
            {
                _currentTime += Time.deltaTime;
            }
        }
    }
}
