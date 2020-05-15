using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] new Rigidbody rigidbody;
    [SerializeField] float lifeTime = 15f;
    [SerializeField] [Range(0f, 1f)] float survivalChance = 0.1f;
    [SerializeField] new Collider collider;

    private float damage = 0f;
    public void Init(float damage, float speed, Quaternion rotation, Collider[] colliders)
    {
        this.damage = damage;
        rigidbody.isKinematic = false;

        rigidbody.rotation = rotation;
        rigidbody.velocity = rigidbody.rotation * (Vector3.forward * speed);
        Destroy(gameObject, lifeTime);

        foreach (Collider c in colliders)
            Physics.IgnoreCollision(collider, c);
    }

    private void FixedUpdate()
    {
        rigidbody.rotation = Quaternion.LookRotation(rigidbody.velocity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var damagable = collision.transform.GetComponentInParent<IDamagable>();
        if (damagable != null)
            damagable.Damage(damage);

        //Destroy(gameObject);
    }
}
