using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponInstance : ModdableItemInstance
{
    public WeaponBase WeaponBase => weaponBase;
    [SerializeField] WeaponBase weaponBase = null;

    public WeaponInstance(WeaponBase weaponBase) : base(weaponBase)
    {
        this.weaponBase = weaponBase;
    }
}
