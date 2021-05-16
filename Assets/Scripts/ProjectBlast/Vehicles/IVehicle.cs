using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBlast.Vehicles
{
    public interface IVehicle
    {
        bool Enter(Actor pilot);
        bool Exit();
    }
}