﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] new Rigidbody rigidbody = null;
    [SerializeField] GameObject cameraPivot = null;

    [Header("Movement Options")]
    [SerializeField] float speed = 10f;     //Movement speed
    [SerializeField] float boost = 0f;      //Sprinting speed, added to movement speed
    [SerializeField] float crouchPenalty = 0f;
    [SerializeField] bool isSprint = false;
    [SerializeField] bool isCrouch = false;

    [Header("Camera Options")]
    [SerializeField] float sensitivity = 2f;
    [SerializeField] float maxXRotation = 80f;
    [SerializeField] float minXRotation = -80f;
    [SerializeField] float cameraHeight = 1f;
    [SerializeField] float cameraCrouch = 0.5f;

    [Header("Interaction Options")]
    [SerializeField] float interactionDistance = 3.5f;

    private new Camera camera; 
    private float cameraRotation = 0f;
    private bool lookEnabled = true;

    private IInteractable interactionTarget = null;

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        Rotate();
        Interact();
        Sprint();
        Crouch();
    }

    private void FixedUpdate()
    {
        Move(Time.fixedDeltaTime);
    }

    private void Rotate()
    {
        if (lookEnabled == false)
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

        Vector3 movement = Vector3.ClampMagnitude(new Vector3(xInput, 0f, yInput), 1f);
        //Determines movement speed based on sprinting and sprint speed
        Vector3 verticalVelocity = new Vector3(0f, rigidbody.velocity.y, 0f);
        rigidbody.velocity = rigidbody.rotation * (movement * ((speed + boost) * (1 - crouchPenalty))) + verticalVelocity;
    }

    private void Sprint()
    {
        bool boostOn = Input.GetButtonDown("Sprint");
        //If the sprint key is pressed, toggle sprint
        if(boostOn && !isCrouch)
        {
            isSprint = !isSprint;
            boost = 5f;
            isCrouch = false;
        }
        //If not moving, stop sprinting
        if (!Input.GetButton("Horizontal") && !Input.GetButton("Vertical") || !isSprint)
        {
            isSprint = false;
            boost = 0f;
        }
    }

    private void Crouch()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            isCrouch = !isCrouch;
            isSprint = false;
            cameraPivot.transform.localPosition = new Vector3(0f, cameraCrouch, 0f);
            crouchPenalty = 0.7f;
        }
        if (Input.GetButtonUp("Crouch"))
        {
            isCrouch = false;
            cameraPivot.transform.localPosition = new Vector3(0f, cameraHeight, 0f);
            crouchPenalty = 0f;
        }
    }

    private void Interact()
    {
        Transform cameraT = camera.transform;
        Vector3 eyePos = this.transform.position + (Vector3.up * 1.65f); // Approx where the eyes would be

        // Approx length for ray
        float rayCastLength = ((cameraT.position - eyePos).magnitude * 1.25f) + interactionDistance;

        RaycastHit hit;
        if(Physics.Raycast(cameraT.position, cameraT.forward, out hit, rayCastLength))
        {
            // Return if distance is larger
            if ((hit.point - eyePos).sqrMagnitude > Mathf.Pow(interactionDistance, 2))
                return;
            interactionTarget = hit.collider.GetComponentInParent<IInteractable>();
        }

        if (interactionTarget != null && Input.GetButtonDown("Interact"))
            interactionTarget.Interact();
    }
}