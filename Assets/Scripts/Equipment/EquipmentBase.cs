using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/New Equipment")]
public class EquipmentBase : ItemBase
{
    [SerializeField] EquipmentSlot equipmentSlot = EquipmentSlot.PRIMARY_WEAPON;

    public EquipmentSlot GetEquipmentSlot()
    {
        return equipmentSlot;
    }
}
