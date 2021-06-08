using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBlast.CameraScripts
{
    public class SpaceshipCameraController : CameraController
    {
        [SerializeField] Transform target;
        [SerializeField] Vector3 offset = Vector3.zero;

        [SerializeField] float rotationSmoothing = 6f;
        [SerializeField] float rotationEffectSmoothing = 4f;
        [SerializeField] float rotationMovementScale = 1.5f;
        Quaternion targetPreviousRotation;
        Quaternion cameraPreviousRotation;

        public override void Enable()
        {
            targetPreviousRotation = target.rotation;
        }

        void Update()
        {
            targetPreviousRotation = Quaternion.Slerp(targetPreviousRotation, target.rotation, rotationEffectSmoothing * Time.deltaTime);

            Quaternion rotationEffect = Quaternion.Inverse(Quaternion.Inverse(target.rotation) * targetPreviousRotation);


            cameraPreviousRotation = Quaternion.Slerp(cameraPreviousRotation, target.rotation * rotationEffect, Time.deltaTime * rotationSmoothing);
            cameraTransform.rotation = cameraPreviousRotation;

            Vector3 rotationMovement = rotationEffect * Vector3.forward * rotationMovementScale;
            rotationMovement.z = 0f;
            rotationMovement.z = -rotationMovement.magnitude;

            cameraTransform.position = target.position + target.rotation * (offset + rotationMovement);

        }
    }
}