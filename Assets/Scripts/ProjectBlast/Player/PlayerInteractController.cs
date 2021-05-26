using ProjectBlast.Engine;
using ProjectBlast.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace ProjectBlast.PlayerScripts
{
    public class PlayerInteractController : MonoBehaviour
    {
        [SerializeField] Player player = null;

        [Header("Interaction Options")]
        [SerializeField] float interactionDistance = 3.5f;
        [SerializeField] float holdToGrabLength = 0.35f;
        [SerializeField] float grabbedItemDistanceMult = 0.5f;

        private IInteractable interactionTarget = null;
        private IGrabbable grabTarget = null;
        private float grabTimer = -100f;
        private bool grabbed = false;

        private Coroutine interactLoop;

        private void OnEnable()
        {
            InputManager.mainInput.Player.Interact.started += InteractStart;
            InputManager.mainInput.Player.Interact.canceled += InteractEnd;
        }

        private void OnDisable()
        {
            InputManager.mainInput.Player.Interact.started -= InteractStart;
            InputManager.mainInput.Player.Interact.canceled -= InteractEnd;
        }

        private void InteractStart(CallbackContext context)
        {
            grabTimer = Time.time;
            interactLoop = StartCoroutine(InteractionLoop());
        }

        private void InteractEnd(CallbackContext context)
        {
            StopCoroutine(interactLoop);
            if (Time.time - grabTimer <= holdToGrabLength)
                HandleInteraction();

            ReleaseGrab();
        }

        private IEnumerator InteractionLoop()
        {
            while (true)
            {
                if (grabbed) {
                    UpdateGrab();
                }
                else if (Time.time - grabTimer > holdToGrabLength) {
                    HandleGrab();
                }

                yield return null;
            }
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

            if (interactionTarget != null && false)
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
            if (grabTarget == null)
            {
                ReleaseGrab();
            }

            Transform cameraT = Camera.main.transform;
            Vector3 eyePos = transform.position + Vector3.up * 1.65f; // Approx where the eyes would be
            float offset = (cameraT.position - eyePos).magnitude * 1.25f + interactionDistance;
            grabTarget.Rigidbody.MovePosition(cameraT.position + cameraT.forward * offset * grabbedItemDistanceMult);
        }

        private void ReleaseGrab()
        {
            if (grabTarget != null)
            {
                grabTarget.SetGrabbed(false);
            }

            grabTarget = null;
            grabbed = false;
            return;
        }
    }
}