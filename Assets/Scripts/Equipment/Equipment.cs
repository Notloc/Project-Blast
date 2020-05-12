using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{
    [SerializeField] EquipmentSlot equipmentSlot = EquipmentSlot.PRIMARY_WEAPON;

    private EquipmentBase equipmentBase;

    public override void Init(ItemBase itemBase)
    {
        equipmentBase = (EquipmentBase)itemBase;
        base.Init(itemBase);
    }

    public EquipmentSlot GetEquipmentSlot()
    {
        return equipmentSlot;
    }

    public EquipmentBase GetEquipmentBase()
    {
        return equipmentBase;
    }
}
