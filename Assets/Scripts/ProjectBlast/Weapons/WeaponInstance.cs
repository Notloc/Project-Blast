using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponInstance : ModdableItemInstance
{
    public WeaponBase WeaponBase => weaponBase;
    [SerializeField] WeaponBase weaponBase = null;

    public WeaponInstance(WeaponBase weaponBase, List<ItemModData> attachments = null) : base(weaponBase, attachments)
    {
        this.weaponBase = weaponBase;
    }
}
