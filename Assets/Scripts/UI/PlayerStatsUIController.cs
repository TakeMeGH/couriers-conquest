using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CC.Core.Data.Dynamic;

namespace CC.UI
{
    public class PlayerStatsUIController : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] PlayerStatsSO _playerStatsSO;
        [Header("UI Component")]
        [SerializeField] Slider _healthSlider;
        [SerializeField] Slider _easeHealthSlider;
        [SerializeField] Slider _staminaSlider;
        [SerializeField] Slider _expSlider;
        [SerializeField] TMP_Text _levelText;
        [Header("Health Logic")]
        [SerializeField] float _lerpTime;
        float _currentTime;

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            _healthSlider.maxValue = _playerStatsSO.GetValue(mainStat.Health);
            _healthSlider.minValue = 0;
            _healthSlider.value = _playerStatsSO.GetInstanceValue(mainStat.Health);

            _easeHealthSlider.maxValue = _playerStatsSO.GetValue(mainStat.Health);
            _easeHealthSlider.minValue = 0;
            _easeHealthSlider.value = _playerStatsSO.GetInstanceValue(mainStat.Health);

            _staminaSlider.maxValue = _playerStatsSO.GetValue(mainStat.Stamina);
            _staminaSlider.minValue = 0;
            _staminaSlider.value = _playerStatsSO.GetInstanceValue(mainStat.Stamina);

            _playerStatsSO.OnStatChange += OnStatChange;
        }

        void OnStatChange()
        {
            HealthAdjust();
            StaminaAdjust();
        }

        void HealthAdjust()
        {
            if (_healthSlider.value != _playerStatsSO.GetInstanceValue(mainStat.Health))
            {
                _currentTime = 0f;
                _healthSlider.value = _playerStatsSO.GetInstanceValue(mainStat.Health);
            }
        }

        void StaminaAdjust()
        {
            _staminaSlider.value = _playerStatsSO.GetInstanceValue(mainStat.Stamina);
        }

        private void Update()
        {
            if (_currentTime < _lerpTime)
            {
                _currentTime += Time.deltaTime;
                _easeHealthSlider.value = Mathf.Lerp(_easeHealthSlider.value, _healthSlider.value, _currentTime / _lerpTime);
            }
        }

    }
}
