using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] new Collider collider = null;
    [SerializeField] new Rigidbody rigidbody = null;

    private float damage;

    public void Init(float damage, float speed, Collider[] colliders)
    {
        // Impossible to shoot self
        foreach (Collider c in colliders)
            Physics.IgnoreCollision(collider, c);

        this.damage = damage;
        rigidbody.velocity = transform.forward * speed;
    }
}
