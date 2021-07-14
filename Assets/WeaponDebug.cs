using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDebug : MonoBehaviour
{
    [SerializeField] WeaponSlots weaponSlots = null;
    [SerializeField] WeaponInstanceHolder primaryWeaponHolder = null;
    [SerializeField] WeaponController weaponController = null;

    private void Awake()
    {
        WeaponInstance weaponInstance = primaryWeaponHolder.CreateWeaponInstance();
        weaponSlots.SetPrimaryWeapon(weaponInstance);
        weaponController.EquipWeapon(weaponInstance);
    }
}
