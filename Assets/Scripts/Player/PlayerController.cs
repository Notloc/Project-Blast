using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IController
{
    [Header("Required References")]
    [SerializeField] Player player = null;
    [SerializeField] new Rigidbody rigidbody = null;
    [SerializeField] GameObject cameraPivot = null;

    [Header("Movement Options")]
    [SerializeField] float speed = 10f;     //Movement speed
    [SerializeField] float boost = 5f;      //Sprinting speed, added to movement speed
    [SerializeField] bool isSprint = false;

    [Header("Camera Options")]
    [SerializeField] float sensitivity = 2f;
    [SerializeField] float maxXRotation = 80f;
    [SerializeField] float minXRotation = -80f;

    [Header("Interaction Options")]
    [SerializeField] float interactionDistance = 3.5f;
    [SerializeField] float holdToGrabLength = 0.35f;
    [SerializeField] float grabbedItemDistanceMult = 0.5f;

    private InputScript inputScript;

    private new Camera camera; 
    private float cameraRotation = 0f;
    private bool lookEnabled = true;
    private bool controlEnabled = true;

    private IInteractable interactionTarget = null;
    private IGrabbable grabTarget = null;
    private float grabTimer = -100f;
    private bool grabbed = false;

    public void SetControlsActive(bool state)
    {
        controlEnabled = state;
    }

    public void SetInput(InputScript input)
    {
        this.inputScript = input;
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
        Rotate();
        TakeSprintInput();
        UpdateTimers();
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

        Move(Time.fixedDeltaTime);
    }

    private void Rotate()
    {
        if (lookEnabled == false)
            return;

        // Body rotation
        float yRotation = inputScript.GetAxis("CameraYaw")*sensitivity;
        Quaternion deltaRotation = Quaternion.Euler(0f, yRotation, 0f);

        this.transform.rotation = deltaRotation * this.transform.rotation;

        // Camera rotation

        float deltaXRotation = inputScript.GetAxis("CameraPitch")*sensitivity;

        cameraRotation -= deltaXRotation;

        cameraRotation = Mathf.Clamp(cameraRotation, minXRotation, maxXRotation);

        cameraPivot.transform.localRotation = Quaternion.Euler(cameraRotation, 0f, 0f);
    }

    private void Move(float deltaTime)
    {
        float xInput = inputScript.GetAxis("PlayerStrafe");
        float yInput = inputScript.GetAxis("PlayerForward");

        Vector3 movement = Vector3.ClampMagnitude(new Vector3(xInput, 0f, yInput), 1f);
        //Determines movement speed based on sprinting and sprint speed
        if(isSprint)
        {
            movement *= (speed + boost);
        }
        else
        {
            movement *= speed;
        }

        Vector3 verticalVelocity = new Vector3(0f, rigidbody.velocity.y, 0f);
        rigidbody.velocity = rigidbody.rotation * movement + verticalVelocity;
    }

    private void TakeSprintInput()
    {
        bool boostOn = inputScript.GetButtonDown("PlayerSprint");
        //If the sprint key is pressed, toggle sprint
        if(boostOn)
        {
            isSprint = !isSprint;
        }
        //If not moving, stop sprinting
        if (rigidbody.velocity.Equals(Vector3.zero))
        {
            isSprint = false;
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
        Transform cameraT = camera.transform;
        Vector3 eyePos = this.transform.position + (Vector3.up * 1.65f); // Approx where the eyes would be

        // Approx length for ray
        float rayCastLength = ((cameraT.position - eyePos).magnitude * 1.25f) + interactionDistance;

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
        Transform cameraT = camera.transform;
        Vector3 eyePos = this.transform.position + (Vector3.up * 1.65f); // Approx where the eyes would be

        // Approx length for ray
        float rayCastLength = ((cameraT.position - eyePos).magnitude * 1.25f) + interactionDistance;

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

        Transform cameraT = camera.transform;
        Vector3 eyePos = this.transform.position + (Vector3.up * 1.65f); // Approx where the eyes would be
        float offset = ((cameraT.position - eyePos).magnitude * 1.25f) + interactionDistance;
        grabTarget.Rigidbody.MovePosition(cameraT.position + (cameraT.forward * offset * grabbedItemDistanceMult));
    }
}
