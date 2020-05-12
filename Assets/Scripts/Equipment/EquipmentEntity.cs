using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentEntity : MonoBehaviour
{
    private GameObject item3D;
    private Equipment equipment;
    private Weapon weapon;
    

    public void Init(Equipment equipment)
    {
        this.equipment = equipment;
        item3D = Instantiate(equipment.GetModel(), this.transform);
        weapon = item3D.GetComponent<Weapon>();
    }

    public Weapon GetWeapon()
    {
        return weapon;
    }
}
