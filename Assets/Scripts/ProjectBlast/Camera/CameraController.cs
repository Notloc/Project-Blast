using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBlast.CameraScripts
{
    public abstract class CameraController : MonoBehaviour
    {
        protected Transform cameraTransform;
        protected new Camera camera;

        private void Start()
        {
            camera = Camera.main;
            cameraTransform = camera.transform;
        }

        public virtual void Enable()
        {
            enabled = true;
        }

        public virtual void Disable()
        {
            enabled = false;
        }
    }
}