using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipCameraController : CameraController
{
    public override CameraControllerType CameraControllerType => CameraControllerType.SPACESHIP;

    [SerializeField] Vector3 offset = Vector3.zero;

    void Update()
    {
        cameraTransform.position = target.position + target.rotation * offset;
        cameraTransform.rotation = target.rotation;
    }
}
