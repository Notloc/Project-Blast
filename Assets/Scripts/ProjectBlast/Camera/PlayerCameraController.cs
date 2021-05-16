using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : CameraController
{
    public override CameraControllerType CameraControllerType => CameraControllerType.PLAYER;
    
    [Header("Camera Options")]
    [SerializeField] float sensitivity = 2f;
    [SerializeField] float maxXRotation = 80f;
    [SerializeField] float minXRotation = -80f;

    [SerializeField] Vector3 height = Vector3.up;
    [SerializeField] Vector3 offset = Vector3.zero;
    
    //[SerializeField] float cameraHeight = 1f;
    //[SerializeField] float cameraCrouch = 0.5f;

    private Vector2 cameraRotation;

    private void Update()
    {
        // Move
        Vector3 basePosition = target.position + height;

        // Rotate
        float deltaXRotation = -Input.GetAxis("Mouse Y") * sensitivity;
        float deltaYRotation = Input.GetAxis("Mouse X") * sensitivity;

        cameraRotation.x = Mathf.Clamp(cameraRotation.x + deltaXRotation, minXRotation, maxXRotation);
        cameraRotation.y = cameraRotation.y + deltaYRotation % 360f;

        Quaternion rotation = Quaternion.Euler(cameraRotation.x, cameraRotation.y, 0f);

        cameraTransform.position = basePosition + (rotation * offset);


        cameraTransform.rotation = rotation;
    }

}
