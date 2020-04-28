using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] Rigidbody playerRBody = null;
    [SerializeField] Camera targetCamera = null;
    [SerializeField] GameObject cameraPivot = null;

    [Header("Movement Options")]
    [SerializeField] float speed = 10f;

    [Header("Camera Options")]
    [SerializeField] float sensitivity = 2f;
    [SerializeField] float maxXRotation = 80f;
    [SerializeField] float minXRotation = -80f;

    public bool LookEnabled = true;

    private float cameraRotation = 0f;

    void Update()
    {
        Rotate();
    }

    private void FixedUpdate()
    {
        Move(Time.fixedDeltaTime);
    }

    private void Rotate()
    {
        if (LookEnabled == false)
            return;

        // Body rotation
        float yRotation = Input.GetAxis("Mouse X")*sensitivity;
        Quaternion deltaRotation = Quaternion.Euler(0f, yRotation, 0f);

        this.transform.rotation = deltaRotation * this.transform.rotation;

        // Camera rotation

        float deltaXRotation = Input.GetAxis("Mouse Y")*sensitivity;

        cameraRotation -= deltaXRotation;

        cameraRotation = Mathf.Clamp(cameraRotation, minXRotation, maxXRotation);

        cameraPivot.transform.localRotation = Quaternion.Euler(cameraRotation, 0f, 0f);
    }

    private void Move(float deltaTime)
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        Vector3 movement = Vector3.ClampMagnitude(new Vector3(xInput, 0f, yInput), 1f)  * speed;
        
        playerRBody.velocity = playerRBody.rotation * movement;
    }
}