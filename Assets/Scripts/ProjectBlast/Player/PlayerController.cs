using Notloc.Utility;
using ProjectBlast.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBlast.PlayerScripts
{
    public class PlayerController : MonoBehaviour
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

        private bool controlEnabled = true;
        private new Camera camera;

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

            Sprint();
            Crouch();
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
            Vector3 direction = rigidbody.velocity;
            if (direction == Vector3.zero)
                return;

            Quaternion cameraRotationFlat = camera.transform.rotation.Flatten();

            Quaternion rotation = Quaternion.LookRotation(cameraRotationFlat * direction);
            rotation = Quaternion.Slerp(rigidbody.rotation, rotation, Time.fixedDeltaTime * rotationSmoothing);
            rigidbody.MoveRotation(rotation);
        }

        private void Move(float deltaTime)
        {
            float xInput = Input.GetAxis("Horizontal");
            float yInput = Input.GetAxis("Vertical");

            Vector3 movement = Vector3.ClampMagnitude(new Vector3(xInput, 0f, yInput), 1f);
            //Determines movement speed based on sprinting and sprint speed
            Vector3 verticalVelocity = new Vector3(0f, rigidbody.velocity.y, 0f);
            rigidbody.velocity = camera.transform.rotation.Flatten() * (movement * ((speed + boost) * (1 - crouchPenalty))) + verticalVelocity;
        }

        private void Sprint()
        {
            bool boostOn = false;
            //If the sprint key is pressed, toggle sprint
            if (boostOn && !isCrouch)
            {
                isSprint = !isSprint;
                boost = 5f;
                isCrouch = false;
            }
            //If not moving, stop sprinting
            if (false && false || !isSprint)
            {
                isSprint = false;
                boost = 0f;
            }
        }

        private void Crouch()
        {
            if (false)
            {
                isCrouch = !isCrouch;
                isSprint = false;
                crouchPenalty = 0.7f;
            }
            if (false)
            {
                isCrouch = false;
                crouchPenalty = 0f;
            }
        }

        
    }
}