using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Inventory
{
    [CreateAssetMenu(fileName = "WeaponItem", menuName = "Items/Weapon", order = 3)]
    public class WeaponItem : ABaseItem
    {
        public int weaponLevel;
        public float attackWeapon;
        public float healthWeapon;
        public float costSell;

        public override ItemType GetItemType()
        {
            return ItemType.Weapon;
        }

        public override void UseItem()
        {
            throw new System.NotImplementedException();
        }
    }
}
