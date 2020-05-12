using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Projectile : MonoBehaviour
{
    [SerializeField] Vector3 gravity = Vector3.zero;

    private float damage = 0f;
    private Vector3 currentSpeed = Vector3.zero;

    public void Init(float damage, float speed, Quaternion rotation)
    {
        this.damage = damage;
        this.transform.rotation = rotation;
        currentSpeed = transform.forward * speed;
    }

    private void FixedUpdate()
    {
        transform.LookAt(transform.position, transform.position + currentSpeed);

        RaycastHit hit;
        if(Physics.Raycast(transform.position, currentSpeed, out hit))
        {
            var damagable = hit.transform.GetComponentInParent<IDamagable>();
            if (damagable != null)
                damagable.Damage(damage);

            Destroy(gameObject);
        }
        else
        {
            currentSpeed += (gravity * Time.fixedDeltaTime);
            transform.position += currentSpeed * Time.fixedDeltaTime;
        }
    }
}
