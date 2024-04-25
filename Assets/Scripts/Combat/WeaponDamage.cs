using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CC.Characters;

namespace CC.Combats
{
    public class WeaponDamage : MonoBehaviour
    {
        [SerializeField] private Collider myCollider;
        [SerializeField] private PlayerControllerStatesMachine _playerController; 

        private int damage;

        private List<Collider> alreadyCollidedWith = new List<Collider>();

        private void OnEnable()
        {
            alreadyCollidedWith.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other == myCollider) { return; }

            if (alreadyCollidedWith.Contains(other)) { return; }

            alreadyCollidedWith.Add(other);

            if (other.TryGetComponent<Health>(out Health health))
            {
                health.DealDamage(damage);
            }
        }

        public void SetAttack(int attackValue)  // Add the attackValue parameter
        {
            // Get the player's base attack value
            float baseAttack = _playerController.playerStatsSO.GetValue(CC.Core.Data.Dynamic.mainStat.AttackValue);

            if (_playerController.equippedWeapon != null)
            {
                // If a weapon is equipped, add its attack value to the base attack value
                this.damage = Mathf.RoundToInt(baseAttack + _playerController.equippedWeapon.attackWeapon);
            }
            else
            {
                // If no weapon is equipped, use the base attack value
                this.damage = Mathf.RoundToInt(baseAttack);
            }   
}


    }

}

