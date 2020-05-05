using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] Player player = null;
    [SerializeField] new Rigidbody rigidbody = null;
    [SerializeField] GameObject cameraPivot = null;

    [Header("Movement Options")]
    [SerializeField] float speed = 10f;

    [Header("Camera Options")]
    [SerializeField] float sensitivity = 2f;
    [SerializeField] float maxXRotation = 80f;
    [SerializeField] float minXRotation = -80f;

    [Header("Interaction Options")]
    [SerializeField] float interactionDistance = 3.5f;
    [SerializeField] float holdToGrabLength = 0.35f;
    [SerializeField] float grabbedItemDistanceMult = 0.5f;

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

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        if (!controlEnabled)
            return;

        Rotate();
        Interact();

        UpdateTimers();
    }

    private void UpdateTimers()
    {
        if (Input.GetButton("Interact"))
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

        Vector3 verticalVelocity = new Vector3(0f, rigidbody.velocity.y, 0f);
        rigidbody.velocity = rigidbody.rotation * movement + verticalVelocity;
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

        if (interactionTarget != null && Input.GetButtonUp("Interact"))
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
        if (grabTarget == null || Input.GetButtonUp("Interact"))
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
