using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Projectile : MonoBehaviour
{
    [SerializeField] Vector3 gravity = Vector3.zero;
    [SerializeField] LayerMask layerMask = 0;
    [SerializeField] float mass = 0.05f;
    [SerializeField] float lifeTime = 8f;

    private float damage = 0f;
    private Vector3 currentSpeed = Vector3.zero;

    public void Init(float damage, float speed, Quaternion rotation)
    {
        this.damage = damage;
        this.transform.rotation = rotation;
        currentSpeed = transform.forward * speed;

        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.LookAt(transform.position, transform.position + currentSpeed);

        RaycastHit hit;
        if(Physics.Raycast(transform.position, currentSpeed, out hit, layerMask))
        {
            var damagable = hit.transform.GetComponentInParent<IDamagable>();
            if (damagable != null)
                damagable.Damage(damage);

            Rigidbody rigidbody = hit.transform.GetComponent<Rigidbody>();
            if (rigidbody)
                rigidbody.AddForce(currentSpeed * mass, ForceMode.Impulse);

            

            Destroy(gameObject);
        }
        else
        {
            currentSpeed += (gravity * Time.deltaTime);
            transform.position += currentSpeed * Time.deltaTime;
        }
    }
}
