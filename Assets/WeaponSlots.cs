using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlots : MonoBehaviour
{
    WeaponInstance primarySlot = null;


    public WeaponInstance GetPrimaryWeapon()
    {
        return primarySlot;
    }

    public void SetPrimaryWeapon(WeaponInstance weaponInstance)
    {
        primarySlot = weaponInstance;
    }

}
