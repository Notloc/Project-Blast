using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class SpaceshipController : MonoBehaviour, IController
{
    [SerializeField] Spaceship spaceship = null;
    [SerializeField] protected new Rigidbody rigidbody = null;

    [SerializeField] float pitchMultiplier = 1f;
    [SerializeField] float yawMultiplier = 0.25f;
    [SerializeField] float rollMultiplier = 3f;

    private InputScript inputScript;
    private bool controlsActive = true;

    Vector3 rotationInput;
    bool takeOff;
    bool landShip;
    bool exitShip;

    private float throttle = 0f;

    public void SetControlsActive(bool state)
    {
        controlsActive = state;
    }

    public void SetInput(InputScript input)
    {
        inputScript = input;
        if (input)
            TakeInput();
    }

    private void TakeInput()
    {
        rotationInput = new Vector3(inputScript.GetAxis("ShipPitch") * pitchMultiplier, inputScript.GetAxis("ShipYaw") * yawMultiplier, inputScript.GetAxis("ShipRoll") * rollMultiplier);
        takeOff = inputScript.GetButton("ShipTakeOff");
        landShip = inputScript.GetButton("ShipLand");
        exitShip = inputScript.GetButton("ShipExit");

        if (spaceship.ShipState == SpaceShipState.FLYING)
        {
            if (inputScript.GetButton("ShipThrottleUp"))
                throttle += 0.7f * Time.deltaTime;

            if (inputScript.GetButton("ShipThrottleDown"))
                throttle -= 0.7f * Time.deltaTime;

            throttle = Mathf.Clamp01(throttle);
        }
    }

    private void Update()
    {
        if (!controlsActive)
            return;

        TakeInput();
    }

    private void FixedUpdate()
    {
        if (!controlsActive)
            return;

        switch (spaceship.ShipState)
        {
            case SpaceShipState.LANDED:
                Landed();
                break;

            case SpaceShipState.FLYING:
                Flying();
                break;
        }
    }

    private void Landed()
    {
        if (exitShip)
        {
            if (spaceship.Exit())
                return;
        }

        if (takeOff)
        {
            throttle = 0f;
            spaceship.Launch();
        }
    }

    private void Flying()
    {
        Quaternion deltaRotation = Quaternion.Euler(rotationInput.y, rotationInput.x, rotationInput.z);

        rigidbody.AddRelativeForce(Vector3.forward * throttle * spaceship.MaxSpeed, ForceMode.Acceleration);
        rigidbody.AddRelativeTorque(rotationInput * spaceship.TurnSpeed, ForceMode.Acceleration);

        if (landShip)
        {
            spaceship.TryToLand();
        }
    }
}
