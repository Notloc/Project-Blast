using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrabbable : IGameObject
{
    Rigidbody Rigidbody { get; }
    void SetGrabbed(bool isGrabbed);
}
