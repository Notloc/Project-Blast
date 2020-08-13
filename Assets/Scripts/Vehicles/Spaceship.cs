using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour, IVehicle, IInteractable
{
    [SerializeField] new Rigidbody rigidbody = null;
    [SerializeField] SpaceshipController shipController = null;
    [SerializeField] SpaceShipState shipState = SpaceShipState.LANDED;
    
    [SerializeField] Transform seatPoint = null;
    [SerializeField] Transform exitPoint = null;

    [SerializeField] float launchHeight = 8.5f;
    [SerializeField] float launchTime = 3.5f;

    [SerializeField] float maxSpeed = 100f;

    public SpaceShipState ShipState { get { return shipState; } }
    public float MaxSpeed { get { return maxSpeed; } }

    Actor pilot;
    bool canExit = true;

    private void Awake()
    {
        SetRigidbodyLanded();
    }

    public bool Enter(Actor newPilot)
    {
        if (pilot)
            return false;

        SeatPilot(newPilot);
        newPilot.SetActiveController(shipController);

        if (newPilot as Player)
        {
            CameraManager.Instance.SetCameraController(CameraControllerType.SPACESHIP, transform);
        }

        return true;
    }

    public bool Exit()
    {
        if (!canExit)
            return false;

        if (pilot as Player)
        {
            CameraManager.Instance.SetCameraController(CameraControllerType.PLAYER, pilot.transform);
        }

        RemovePilot();
        return true;
    }

    private void SeatPilot(Actor newPilot)
    {
        pilot = newPilot;
        pilot.Lock();
        pilot.SetCollidersActive(false);
        pilot.transform.SetParent(seatPoint, true);
        pilot.transform.position = seatPoint.position;
    }

    private void RemovePilot()
    {
        pilot.transform.SetParent(null, true);
        pilot.transform.position = exitPoint.position;
        pilot.Unlock();
        pilot.SetCollidersActive(true);
        pilot.SetActiveController(null);
        pilot = null;
    }

    public void Interact(Player player)
    {
        Enter(player);
    }

    public void Launch()
    {
        if (shipState != SpaceShipState.LANDED)
            return;

        StartCoroutine(ShipLaunch());
    }

    private IEnumerator ShipLaunch()
    {
        shipState = SpaceShipState.LAUNCHING;

        Vector3 startPos = rigidbody.position;
        float timer = 0f;
        while (true)
        {
            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;

            float height = (timer / launchTime) * launchHeight;
            rigidbody.MovePosition(startPos + Vector3.up * height);

            if (timer > launchTime)
                break;
        }
        SetRigidbodyFlying();

        shipState = SpaceShipState.FLYING;
    }

    public void Land()
    {
        SetRigidbodyLanded();
        shipState = SpaceShipState.LANDED;
    }

    private void SetRigidbodyFlying()
    {
        rigidbody.isKinematic = false;
    }

    private void SetRigidbodyLanded()
    {
        rigidbody.isKinematic = true;
    }
}

public enum SpaceShipState
{
    LANDED,
    LAUNCHING,
    FLYING,
    LANDING
}