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
        [SerializeField] Vector3 _floatingDamageOffset;
        [SerializeField] Vector3 _floatingDamageRandomOffset;
        PlayerStatsSO _statsSO;
        bool _isBlocking;

        private void OnEnable()
        {
            if (_statsSO != null)
            {
                _statsSO.OnStatChange += SetHealth;
            }
        }

        private void OnDisable()
        {
            if (_statsSO != null)
            {
                _statsSO.OnStatChange -= SetHealth;
            }
        }

        public void SetStats(PlayerStatsSO statsSO)
        {
            _statsSO = statsSO;
            _statsSO.OnStatChange += SetHealth;
            Init();
        }

        public void SetAttackEvent(VoidEventChannelSO attackEvent)
        {
            _onCharacterDamaged = attackEvent;
        }


        private void Init()
        {
            SetHealth();
        }


        public float GetCurrentHealth()
        {
            return _health;
        }

        public void SetBlocking(bool isBlocking)
        {
            _isBlocking = isBlocking;
        }

        public void SetHealth()
        {
            _health = _statsSO.GetInstanceValue(mainStat.Health);
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
                AudioManager.instance.AudioPlayOneShot(AudioManager.instance.ShieldHit, transform.position);

            }
            else
            {
                AudioManager.instance.AudioPlayOneShot(AudioManager.instance.SwordHit, transform.position);
            }
            float calculatedDamage = Mathf.Min(Mathf.RoundToInt(damage * (1 - totalReduction)), _health);

            CreateFloatingDamage(calculatedDamage);

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
        void CreateFloatingDamage(float _damage)
        {
            GameObject tmp = ObjectPooling.SharedInstance.GetPooledObject(PoolObjectType.FloatingDamageText);
            tmp.transform.position = transform.position;
            tmp.transform.position += _floatingDamageOffset;
            tmp.transform.position += new Vector3(Random.Range(-_floatingDamageRandomOffset.x, _floatingDamageRandomOffset.x),
                Random.Range(-_floatingDamageRandomOffset.y, _floatingDamageRandomOffset.y),
                Random.Range(-_floatingDamageRandomOffset.z, _floatingDamageRandomOffset.z));
            tmp.SetActive(true);

            FloatingDamageController _floatingDamageController = tmp.GetComponent<FloatingDamageController>();
            _floatingDamageController.Init(_damage.ToString());

        }

    }
}
