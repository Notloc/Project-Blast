﻿using Notloc.Utility;
using ProjectBlast.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBlast.PlayerScripts
{
    public class PlayerController : MonoBehaviour, IController
    {
        [Header("Required References")]
        [SerializeField] Player player = null;
        [SerializeField] new Rigidbody rigidbody = null;

        [Header("Movement Options")]
        [SerializeField] float speed = 10f;     //Movement speed
        [SerializeField] float boost = 0f;      //Sprinting speed, added to movement speed
        [SerializeField] float crouchPenalty = 0f;
        [SerializeField] bool isSprint = false;
        [SerializeField] bool isCrouch = false;
        [SerializeField] float rotationSmoothing = 5f;

        [Header("Interaction Options")]
        [SerializeField] float interactionDistance = 3.5f;
        [SerializeField] float holdToGrabLength = 0.35f;
        [SerializeField] float grabbedItemDistanceMult = 0.5f;

        private InputScript inputScript;

        private bool controlEnabled = true;

        private IInteractable interactionTarget = null;
        private IGrabbable grabTarget = null;
        private float grabTimer = -100f;
        private bool grabbed = false;

        private new Camera camera;

        public void SetControlsActive(bool state)
        {
            controlEnabled = state;
        }

        public void SetInput(InputScript input)
        {
            inputScript = input;
        }

        private void Start()
        {
            camera = Camera.main;
        }

        private void Update()
        {
            if (!controlEnabled)
                return;
            Interact();
            if (!inputScript)
                return;

            UpdateTimers();
            Sprint();
            Crouch();
        }

        private void UpdateTimers()
        {
            if (inputScript.GetButton("PlayerInteract"))
                grabTimer += Time.deltaTime;
            else
                grabTimer = 0f;
        }

        private void FixedUpdate()
        {
            if (!controlEnabled)
                return;

            Rotate();
            Move(Time.fixedDeltaTime);
        }

        private void Rotate()
        {
            Vector3 direction = new Vector3(
                inputScript.GetAxis("PlayerStrafe"),
                0f,
                inputScript.GetAxis("PlayerForward")
            );
            if (direction == Vector3.zero)
                return;

            Quaternion cameraRotationFlat = camera.transform.rotation.Flatten();

            Quaternion rotation = Quaternion.LookRotation(cameraRotationFlat * direction);
            rotation = Quaternion.Slerp(rigidbody.rotation, rotation, Time.fixedDeltaTime * rotationSmoothing);
            rigidbody.MoveRotation(rotation);
        }

        private void Move(float deltaTime)
        {
            float xInput = inputScript.GetAxis("PlayerStrafe");
            float yInput = inputScript.GetAxis("PlayerForward");

            Vector3 movement = Vector3.ClampMagnitude(new Vector3(xInput, 0f, yInput), 1f);
            //Determines movement speed based on sprinting and sprint speed
            Vector3 verticalVelocity = new Vector3(0f, rigidbody.velocity.y, 0f);
            rigidbody.velocity = camera.transform.rotation.Flatten() * (movement * ((speed + boost) * (1 - crouchPenalty))) + verticalVelocity;
        }

        private void Sprint()
        {
            bool boostOn = inputScript.GetButtonDown("PlayerSprint");
            //If the sprint key is pressed, toggle sprint
            if (boostOn && !isCrouch)
            {
                isSprint = !isSprint;
                boost = 5f;
                isCrouch = false;
            }
            //If not moving, stop sprinting
            if (!inputScript.GetButton("PlayerStrafe") && !inputScript.GetButton("PlayerForward") || !isSprint)
            {
                isSprint = false;
                boost = 0f;
            }
        }

        private void Crouch()
        {
            if (inputScript.GetButtonDown("Crouch"))
            {
                isCrouch = !isCrouch;
                isSprint = false;
                crouchPenalty = 0.7f;
            }
            if (inputScript.GetButtonUp("Crouch"))
            {
                isCrouch = false;
                crouchPenalty = 0f;
            }
        }

        private void Interact()
        {
            if (grabbed)
            {
                UpdateGrab();
                return;
            }

            if (grabTimer < holdToGrabLength)
                HandleInteraction();
            else
                HandleGrab();
        }

        private void HandleInteraction()
        {
            Transform cameraT = Camera.main.transform;
            Vector3 eyePos = transform.position + Vector3.up * 1.65f; // Approx where the eyes would be

            // Approx length for ray
            float rayCastLength = (cameraT.position - eyePos).magnitude * 1.25f + interactionDistance;

            RaycastHit hit;
            if (Physics.Raycast(cameraT.position, cameraT.forward, out hit, rayCastLength))
            {
                // Return if distance is larger
                if ((hit.point - eyePos).sqrMagnitude > Mathf.Pow(interactionDistance, 2))
                    return;
                interactionTarget = hit.collider.GetComponentInParent<IInteractable>();
            }

            if (interactionTarget != null && inputScript.GetButtonUp("PlayerInteract"))
                interactionTarget.Interact(player);
        }

        private void HandleGrab()
        {
            Transform cameraT = Camera.main.transform;
            Vector3 eyePos = transform.position + Vector3.up * 1.65f; // Approx where the eyes would be

            // Approx length for ray
            float rayCastLength = (cameraT.position - eyePos).magnitude * 1.25f + interactionDistance;

            RaycastHit hit;
            if (Physics.Raycast(cameraT.position, cameraT.forward, out hit, rayCastLength))
            {
                // Return if distance is larger
                if ((hit.point - eyePos).sqrMagnitude > Mathf.Pow(interactionDistance, 2))
                    return;
                grabTarget = hit.collider.GetComponentInParent<IGrabbable>();
            }

            if (grabTarget != null)
            {
                grabbed = true;
                grabTarget.SetGrabbed(true);
            }
        }


        private void UpdateGrab()
        {
            if (grabTarget == null || inputScript.GetButtonUp("PlayerInteract"))
            {
                if (grabTarget != null)
                {
                    grabTarget.SetGrabbed(false);
                }

                grabTarget = null;
                grabbed = false;
                return;
            }

            Transform cameraT = Camera.main.transform;
            Vector3 eyePos = transform.position + Vector3.up * 1.65f; // Approx where the eyes would be
            float offset = (cameraT.position - eyePos).magnitude * 1.25f + interactionDistance;
            grabTarget.Rigidbody.MovePosition(cameraT.position + cameraT.forward * offset * grabbedItemDistanceMult);
        }
    }
}