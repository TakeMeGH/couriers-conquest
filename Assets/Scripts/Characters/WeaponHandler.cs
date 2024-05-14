using System.Collections;
using System.Collections.Generic;
using CC.Combats;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] WeaponDamage weaponLogic;

    public void EnableWeapon()
    {
        weaponLogic.EnableWeapon();
    }

    public void DisableWeapon()
    {
        weaponLogic.DisableWeapon();
    }
}
