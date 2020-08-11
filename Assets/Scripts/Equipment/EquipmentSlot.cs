using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the act of equipping and unequipping items into slots
/// </summary>
public class EquipmentSlot : MonoBehaviour
{
    private EquipmentBehaviour equipmentBehaviour = null;
    private IEquipmentItem equipmentItem = null;

    public EquipmentBehaviour GetEquipmentBehaviour()
    {
        return equipmentBehaviour;
    }

    public void Equip(IEquipmentItem equipmentItem)
    {
        bool sameItem = this.equipmentItem == equipmentItem;
        if (!this.equipmentItem.IsNull())
            Unequip();

        if (sameItem)
            return;

        this.equipmentItem = equipmentItem;
        equipmentBehaviour = Instantiate(this.equipmentItem.EquipmentBehaviour, transform);
        equipmentBehaviour.SetEquipment(equipmentItem);
    }

    public void Unequip()
    {
        equipmentItem = null;
        Destroy(equipmentBehaviour.gameObject);
        equipmentBehaviour = null;
    }
}
