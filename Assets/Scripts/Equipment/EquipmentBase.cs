using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class EquipmentBase : ItemBase, IEquipmentItem
{
    [SerializeField] EquipmentBehaviour equipmentBehaviour = null;

    public abstract EquipmentType EquipmentType { get; }
    public EquipmentBehaviour EquipmentBehaviour { get { return equipmentBehaviour; } }
}
