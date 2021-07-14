using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] WeaponBehaviour weaponPrefab = null;
    [SerializeField] Transform weaponParent = null;

    private WeaponInstance activeWeaponInstance;
    private WeaponBehaviour weaponBehaviour;

    public void EquipWeapon(WeaponInstance weaponInstance)
    {
        if (weaponBehaviour != null)
        {
            Destroy(weaponBehaviour.gameObject);
        }

        this.activeWeaponInstance = weaponInstance;
        weaponBehaviour = Instantiate(weaponPrefab, weaponParent);
        weaponBehaviour.SetWeaponInstance(activeWeaponInstance);
    }




    public void PullTrigger()
    {

    }

    public void ReleaseTrigger()
    {

    }



}
