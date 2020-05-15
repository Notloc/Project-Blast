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
    [SerializeField] Collider[] colliders = null;

    public void Fire()
    {
        FireProjectile();
    }

    public void Aim(Vector3 aimStart, Vector3 forward, float zeroing)
    {
        Vector3 muzzleOffset = muzzle.position - transform.position;
        RaycastHit hit;
        if (Physics.Raycast(aimStart, forward, out hit, zeroing))
        {
            transform.LookAt(hit.point - muzzleOffset);
        }
        else
        {
            transform.LookAt(aimStart + (forward * zeroing) - muzzleOffset);
        }
    }

    private void FireProjectile()
    {
        var projectile = Instantiate(projectilePrefab, muzzle.position, transform.rotation);
        projectile.Init(damage, projectileSpeed, transform.rotation, colliders);
    }
}
