using CC.Core.Data.Dynamic;
using CC.Events;
using UnityEngine;
using UnityEngine.Events;

namespace CC.Combats
{
    public class Health : MonoBehaviour
    {
        [SerializeField] VoidEventChannelSO _onCharacterDamaged;
        public UnityEvent OnHealthReachedZero;
        [SerializeField] float _health;
        PlayerStatsSO _statsSO;
        bool _isBlocking;

        public void SetStats(PlayerStatsSO statsSO)
        {
            _statsSO = statsSO;
            Init();
        }

        private void Init()
        {
            _health = _statsSO.GetInstanceValue(mainStat.Health);
        }


        public float GetCurrentHealth()
        {
            return _health;
        }

        public void SetBlocking(bool isBlocking)
        {
            _isBlocking = isBlocking;
        }



        public float DealDamage(int damage)
        {
            Debug.Log("Received Damage: " + damage);

            if (_health == 0) { return 0; }

            float totalReduction = _statsSO.GetDamageReduction();

            if (_isBlocking)
            {
                float shieldValue = _statsSO.GetValue(mainStat.ShieldValue);
                totalReduction += shieldValue / 100f;
            }
            float calculatedDamage = Mathf.Min(Mathf.RoundToInt(damage * (1 - totalReduction)), _health);

            if (calculatedDamage > 0)
            {
                _onCharacterDamaged?.RaiseEvent();
            }

            _health = Mathf.Max(_health - calculatedDamage, 0);
            _statsSO.SetInstanceValue(mainStat.Health, _health);
            if (_health == 0)
            {
                OnHealthReachedZero.Invoke();
            }

            Debug.Log("Calculated Damage after Reduction: " + calculatedDamage);
            Debug.Log("Player Health after Damage: " + _health);

            return calculatedDamage;
        }
    }
}
