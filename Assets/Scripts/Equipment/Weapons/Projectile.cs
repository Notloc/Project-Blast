using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] new Collider collider = null;
    [SerializeField] new Rigidbody rigidbody = null;
    [SerializeField] TrailRenderer trailRenderer = null;
    [SerializeField] int richochetCount = 3;
    
    private float damage;

    public void Init(float damage, float speed, Collider[] colliders)
    {
        this.damage = damage;
        
        // Impossible to shoot self
        foreach (Collider c in colliders)
            Physics.IgnoreCollision(collider, c);

        trailRenderer.emitting = true;
        rigidbody.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (richochetCount <= 0)
            return;

        IDamagable damagable = collision.collider.GetComponent<IDamagable>();
        if (!damagable.IsNull())
        {
            damagable.Damage(damage);
        }

        richochetCount--;
        trailRenderer.emitting = richochetCount > 0;
    }
}
