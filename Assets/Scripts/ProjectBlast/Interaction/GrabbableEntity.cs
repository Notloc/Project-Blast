using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableEntity : MonoBehaviour, IGrabbable
{
    [SerializeField] protected new Rigidbody rigidbody = null;
    public Rigidbody Rigidbody { get { return rigidbody; } }

    public void SetGrabbed(bool isGrabbed)
    {
        if (isGrabbed)
        {
            Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            Rigidbody.useGravity = false;
        }
        else
        {
            Rigidbody.interpolation = RigidbodyInterpolation.None;
            Rigidbody.useGravity = true;
        }
    }
}
