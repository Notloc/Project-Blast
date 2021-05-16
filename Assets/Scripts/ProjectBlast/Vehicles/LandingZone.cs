using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBlast.Vehicles
{
    [RequireComponent(typeof(Collider))]
    public class LandingZone : MonoBehaviour
    {
        [SerializeField] Transform landingPoint = null;
        public Vector3 GetLandingPosition()
        {
            return landingPoint.position;
        }
    }
}