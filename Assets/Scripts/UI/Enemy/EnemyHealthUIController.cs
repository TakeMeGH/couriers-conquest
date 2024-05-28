using System.Collections;
using System.Collections.Generic;
using CC.Core.Data.Dynamic;
using CC.Events;
using UnityEngine;
using UnityEngine.UI;

namespace CC.UI
{
    public class EnemyHealthUIController : MonoBehaviour
    {
        [SerializeField] Slider _healthSlider;
        [SerializeField] Slider _easeHealthSlider;
        [SerializeField] float _lerpTime;
        PlayerStatsSO _statsSO;
        float _currentTime;

        public void SetStats(PlayerStatsSO statsSO)
        {
            _statsSO = statsSO;
            _statsSO.OnStatChange += OnStatChange;
            Init();
        }

        public void Init()
        {
            _healthSlider.maxValue = _statsSO.GetValue(mainStat.Health);
            _easeHealthSlider.maxValue = _statsSO.GetValue(mainStat.Health);

            _healthSlider.minValue = 0;
            _easeHealthSlider.minValue = 0;

            _healthSlider.value = _statsSO.GetInstanceValue(mainStat.Health);
            _easeHealthSlider.value = _statsSO.GetInstanceValue(mainStat.Health);
        }

        void OnStatChange()
        {
            if (_healthSlider.value != _statsSO.GetInstanceValue(mainStat.Health))
            {
                _currentTime = 0f;
                _healthSlider.value = _statsSO.GetInstanceValue(mainStat.Health);
            }
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
