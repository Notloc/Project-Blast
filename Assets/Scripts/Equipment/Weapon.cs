using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Projectile projectilePrefab = null;
    [SerializeField] Transform muzzle = null;
    [Space]
    [SerializeField] float damage = 1f;
    [SerializeField] float projectileSpeed = 1f;

    public void Fire()
    {
        FireProjectile();
    }

    private void FireProjectile()
    {
        var projectile = Instantiate(projectilePrefab, muzzle.position, Quaternion.identity);

        Quaternion aimDirection = GetPlayerAim();
        projectile.Init(damage, projectileSpeed, aimDirection);
    }

    private Quaternion GetPlayerAim()
    {
        return Quaternion.LookRotation(Camera.main.transform.forward);
    }
}
