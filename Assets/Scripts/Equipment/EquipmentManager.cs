using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public EquipmentSlot gunSlot;

    private GunBehaviour equippedGun;

    public void Equip(IEquipmentItem equipment)
    {
        if (equipment.EquipmentType == EquipmentType.GUN)
            EquipGun(equipment);
    }

    public void Unequip(EquipmentType equipmentType)
    {
        if (equipmentType == EquipmentType.GUN)
            gunSlot.Unequip();
    }

    private void EquipGun(IEquipmentItem equipment)
    {
        gunSlot.Equip(equipment);
        equippedGun = gunSlot.GetEquipmentBehaviour() as GunBehaviour;
    }
}
