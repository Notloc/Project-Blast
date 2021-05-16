using Notloc.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBlast.Interaction
{
    public interface IGrabbable : IGameObject
    {
        Rigidbody Rigidbody { get; }
        void SetGrabbed(bool isGrabbed);
    }
}