using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentItem : Item, IEquipmentItem
{
    public abstract EquipmentType EquipmentType { get; }
    public EquipmentBehaviour EquipmentBehaviour { get; }
}
