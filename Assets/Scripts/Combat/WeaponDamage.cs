using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CC.Characters;
using CC.Core.Data.Dynamic;

namespace CC.Combats
{
    public class WeaponDamage : MonoBehaviour
    {
        [SerializeField] LayerMask _targetLayer;
        [SerializeField] GameObject _onHitVFX;
        PlayerStatsSO _statsSO;
        HitPause _hitPause;
        bool _canHitPause;
        int _damage;
        Collider _collider;
        private List<Collider> _alreadyCollidedWith = new List<Collider>();


        void Start()
        {
            _collider = GetComponent<Collider>();
            _hitPause = GetComponent<HitPause>();
        }
        public void SetStats(PlayerStatsSO statsSO)
        {
            _statsSO = statsSO;
        }

        public void EnableWeapon()
        {
            _alreadyCollidedWith.Clear();
            _collider.enabled = true;
            _canHitPause = true;
        }

        public void DisableWeapon()
        {
            _collider.enabled = false;
        }


        private void OnEnable()
        {
            _alreadyCollidedWith.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((_targetLayer & (1 << other.gameObject.layer)) == 0) { return; }

            if (_alreadyCollidedWith.Contains(other)) { return; }

            _alreadyCollidedWith.Add(other);

            if (other.TryGetComponent(out Health health))
            {
                health.DealDamage(_damage);
                if (other.TryGetComponent(out OnHitSparks onHitSparks))
                {
                    onHitSparks.OnHit(transform.position, _onHitVFX);
                }
                if (_canHitPause)
                {
                    _canHitPause = false;
                    _hitPause?.OnHit();
                }
            }
        }

        public void SetAttack()
        {
            _damage = Mathf.RoundToInt(_statsSO.GetValue(mainStat.AttackValue));
        }

    }
}
