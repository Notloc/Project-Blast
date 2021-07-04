using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemModData
{
    public ItemInstance mod;
    public string slotName;

    public ItemModData(ItemInstance mod, string slotName)
    {
        this.mod = mod;
        this.slotName = slotName;
    }
}
