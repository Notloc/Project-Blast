using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentBehaviour : MonoBehaviour
{
    public abstract void SetEquipment(IEquipmentItem equipment);
}
