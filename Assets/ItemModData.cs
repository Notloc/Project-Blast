using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemModData
{
    public ItemInstance itemInstance;
    public string modSlotName;

    public ItemModData(ItemInstance itemInstance, string modSlotName)
    {
        this.itemInstance = itemInstance;
        this.modSlotName = modSlotName;
    }
}
