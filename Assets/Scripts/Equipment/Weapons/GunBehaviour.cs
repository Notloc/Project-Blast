using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehaviour : EquipmentBehaviour
{
    GunItem item;

    public override void SetEquipment(IEquipmentItem equipment)
    {
        item = equipment as GunItem;
    }
}
