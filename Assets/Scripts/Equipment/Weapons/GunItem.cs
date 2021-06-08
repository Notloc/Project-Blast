using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunItem : EquipmentItem
{
    public override EquipmentType EquipmentType => EquipmentType.GUN;
    public bool IsAutomatic { get { return gunBase.IsAutomatic; } }
    public Projectile Projectile { get { return gunBase.Projectile; } }
    public float Damage { get { return gunBase.Damage; } }
    public float ProjectileSpeed { get { return gunBase.ProjectileSpeed; } }
    public float FireRate { get { return gunBase.FireRate; } }

    GunBase gunBase;

    public override void Init(ItemBase itemBase)
    {
        base.Init(itemBase);
        gunBase = (GunBase)itemBase;
    }
}
