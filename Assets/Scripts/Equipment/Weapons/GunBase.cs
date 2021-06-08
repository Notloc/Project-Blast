using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/New Gun")]
public class GunBase : EquipmentBase
{
    public override EquipmentType EquipmentType => EquipmentType.GUN;

    [SerializeField] float damage = 10f;
    [SerializeField] float fireRate = 5f;
    [SerializeField] Projectile projectile = null;
    [SerializeField] float projectileSpeed = 50f;
    [SerializeField] bool isAutomatic = false;
    public Projectile Projectile { get { return projectile; } }
    public float ProjectileSpeed { get { return projectileSpeed; } }
    public float Damage { get { return damage; } }
    public float FireRate { get { return fireRate; } }
    public bool IsAutomatic { get { return isAutomatic; } }
}
