using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EquipmentManager : MonoBehaviour
{
    [SerializeField] EquipmentSlot gunSlot = null;

    private GunBehaviour equippedGun;
    
    public UnityAction OnEquip;

    public void Equip(IEquipmentItem equipment)
    {
        if (equipment.EquipmentType == EquipmentType.GUN)
            EquipGun(equipment);

        OnEquip?.Invoke();
    }

    public void Unequip(EquipmentType equipmentType)
    {
        if (equipmentType == EquipmentType.GUN)
            gunSlot.Unequip();

        OnEquip?.Invoke();
    }

    private void EquipGun(IEquipmentItem equipment)
    {
        gunSlot.Equip(equipment);
        equippedGun = gunSlot.GetEquipmentBehaviour() as GunBehaviour;
    }

    public GunBehaviour GetGun()
    {
        return equippedGun;
    }
}
