using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    [SerializeField] new Rigidbody rigidbody;
    [SerializeField] Vector3 gravity = Vector3.zero;
    private void FixedUpdate()
    {
        rigidbody.velocity += (gravity * Time.fixedDeltaTime);
    }
}
