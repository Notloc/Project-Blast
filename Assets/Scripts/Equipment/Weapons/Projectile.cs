using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] new Collider collider = null;
    [SerializeField] new Rigidbody rigidbody = null;
    [SerializeField] TrailRenderer trailRenderer = null;
    
    private float damage;
    private bool live;

    public void Init(float damage, float speed, Collider[] colliders)
    {
        this.damage = damage;
        
        // Impossible to shoot self
        foreach (Collider c in colliders)
            Physics.IgnoreCollision(collider, c);

        trailRenderer.emitting = true;
        rigidbody.velocity = transform.forward * speed;
        live = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamagable damagable = collision.collider.GetComponent<IDamagable>();
        if (!damagable.IsNull())
        {
            damagable.Damage(damage);
        }

        trailRenderer.emitting = false;
        live = false;
    }
}
