using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField] protected new Rigidbody rigidbody = null;
    public Rigidbody Rigidbody { get { return rigidbody; } }
    protected bool isLocked;

    public virtual void SetCollidersActive(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider c in colliders)
            c.enabled = state;
    }

    public virtual void Lock()
    {
        rigidbody.isKinematic = true;
        isLocked = true;
    }

    public virtual void Unlock()
    {
        rigidbody.isKinematic = false;
        rigidbody.velocity = Vector3.zero;
        isLocked = false;
    }
}
