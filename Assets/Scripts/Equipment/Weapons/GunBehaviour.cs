using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehaviour : EquipmentBehaviour
{
    [SerializeField] Transform muzzle = null;
    
    public bool IsAutomatic { get { return gun.IsAutomatic; } }

    private GunItem gun;
    private float shotTimer = -100f;

    
    public override void SetEquipment(IEquipmentItem equipment)
    {
        gun = equipment as GunItem;
    }

    public void Fire(Collider[] colliders)
    {
        if (Time.time < shotTimer)
            return;

        Projectile p = Instantiate(gun.Projectile, muzzle.position, muzzle.rotation);
        p.Init(gun.Damage, gun.ProjectileSpeed, colliders);
        shotTimer = Time.time + 1f / gun.FireRate;
    }
}
