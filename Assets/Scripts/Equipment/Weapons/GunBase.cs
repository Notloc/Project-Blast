using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/New Gun")]
public class GunBase : EquipmentBase
{
    public override EquipmentType EquipmentType => EquipmentType.GUN;

    [SerializeField] Projectile projectile = null;
    public Projectile Projectile { get { return projectile; } }

}
