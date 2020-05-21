using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Projectile projectilePrefab = null;
    [SerializeField] Transform muzzle = null;
    [Space]
    [SerializeField] float damage = 1f;
    [SerializeField] float projectileSpeed = 1f;
    [SerializeField] Collider[] colliders = null;
    [SerializeField] float fireRate = 5f;

    private float fireTimer = -100f;

    public void Fire(Collider[] shooterColliders)
    {
        if (fireTimer < Time.time)
            FireProjectile(shooterColliders);
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

    private void FireProjectile(Collider[] shooterColliders)
    {
        fireTimer = Time.time + (1.0f / fireRate);

        Projectile projectile = Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);
        projectile.Init(damage, projectileSpeed, transform.rotation, colliders, shooterColliders);
    }
}
