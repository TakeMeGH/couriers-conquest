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
        //[SerializeField] private HitPause hitPause;


        private int damage;

        public int GetDamage() //si getdamage ini cmn untuk debug log ke playerattackingstate buat ngasih tau jumlah damage yg dikeluarkan player
        {
        return damage;
        }

        

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
                //hitPause.OnHit();
            }
        }

        public void SetAttack()  
        {
            // Get the player's total attack value, including base value and modifiers
            this.damage = Mathf.RoundToInt(_playerController.playerStatsSO.GetValue(CC.Core.Data.Dynamic.mainStat.AttackValue));
        }

    }
}
