using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public EquipmentEntity PrimaryWeapon { get; private set; }
    [SerializeField] Transform primaryWeaponSlot = null;

    public bool Equip(Equipment equipment)
    {
        if (PrimaryWeapon)
            return false;

        if (equipment.GetEquipmentSlot() == EquipmentSlot.PRIMARY_WEAPON)
        {
            EquipWeapon(equipment);
            return true;
        }
        return false;
    }

    private void EquipWeapon(Equipment equipment)
    {
        EquipmentEntity equipmentEntity = Game.Instance.Factories.EquipmentEntityFactory.CreateEquipmentEntity(equipment, transform.position);
        Transform equipmentT = equipmentEntity.transform;

        equipmentT.parent = primaryWeaponSlot;
        equipmentT.localPosition = Vector3.zero;
        equipmentT.localRotation = Quaternion.identity;

        PrimaryWeapon = equipmentEntity;
    }
}
