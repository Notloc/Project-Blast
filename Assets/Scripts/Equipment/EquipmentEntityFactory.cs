using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentEntityFactory : MonoBehaviour
{
    public static EquipmentEntityFactory Instance { get; private set; }
    [SerializeField] EquipmentEntity equipmentEntityPrefab = null;

    private void Awake()
    {
        Instance = this;
    }

    public EquipmentEntity CreateEquipmentEntity(Equipment equipment, Vector3 position)
    {
        EquipmentEntity newEquip = Object.Instantiate(equipmentEntityPrefab, position, Quaternion.identity);
        newEquip.Init(equipment);
        return newEquip;
    }
}
