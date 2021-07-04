using ProjectBlast.CameraScripts;
using ProjectBlast.Interaction;
using ProjectBlast.PlayerScripts;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace ProjectBlast.Vehicles
{
    public class Spaceship : MonoBehaviour, IVehicle, IInteractable
    {
        [SerializeField] new Rigidbody rigidbody = null;
        [SerializeField] SpaceshipCameraController cameraController = null;
        //[SerializeField] SpaceshipController shipController = null;
        [SerializeField] SpaceShipState shipState = SpaceShipState.LANDED;

        [SerializeField] Transform seatPoint = null;
        [SerializeField] Transform exitPoint = null;

        [SerializeField] float launchHeight = 8.5f;
        [SerializeField] float launchTime = 3.5f;

        [SerializeField] float maxSpeed = 100f;
        [SerializeField] float turnSpeed = 5;

        [SerializeField] float landingRadius = 100f;
        [SerializeField] float landingTime = 6f;
        

        public SpaceShipState ShipState { get { return shipState; } }
        public float MaxSpeed { get { return maxSpeed; } }
        public float TurnSpeed { get { return turnSpeed; } }

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
            //newPilot.SetActiveController(shipController);

            if (newPilot as Player)
            {
                CameraManager.Instance.SetActiveCameraController(cameraController);
            }

            return true;
        }

        public bool Exit()
        {
            if (!canExit)
                return false;

            if (pilot as Player)
            {
                CameraManager.Instance.DisableCameraController(cameraController);
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
            //pilot.SetActiveController(null);
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

        public void TryToLand()
        {
            LandingZone zone = FindLandingZone();
            if (!zone)
                return;

            StartCoroutine(Land(zone.GetLandingPosition()));
        }

        private LandingZone FindLandingZone()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, landingRadius);
            foreach (var c in colliders)
            {
                LandingZone zone = c.GetComponent<LandingZone>();
                if (zone)
                {
                    return zone;
                }
            }
            return null;
        }

        private IEnumerator Land(Vector3 landingPosition)
        {
            shipState = SpaceShipState.LANDING;
            SetRigidbodyLanded();

            Vector3 startingPosition = rigidbody.position;
            float timer = 0f;
            while (timer < landingTime)
            {
                yield return null;
                timer += Time.fixedDeltaTime;
                rigidbody.MovePosition(Vector3.Lerp(startingPosition, landingPosition, timer / landingTime));
            }

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
}