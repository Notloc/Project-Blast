using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBlast.CameraScripts
{
    public abstract class CameraController : MonoBehaviour
    {
        public abstract CameraControllerType CameraControllerType { get; }
        [SerializeField] protected Transform cameraTransform;
        protected Transform target;


        public virtual void Enable(Transform target)
        {
            enabled = true;
            this.target = target;
        }

        public virtual void Disable()
        {
            enabled = false;
        }
    }
}