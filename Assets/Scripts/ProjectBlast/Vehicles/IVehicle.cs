using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVehicle
{
    bool Enter(Actor pilot);
    bool Exit();
}
