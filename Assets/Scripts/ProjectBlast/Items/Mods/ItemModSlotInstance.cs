using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemModSlotInstance
{
    public ModdableItemInstance parentModdableItem { get; private set; }
    public ItemModSlotData ModSlotData { get; private set; }

    public ItemInstance Mod => mod;
    private ItemInstance mod;

    public ItemModSlotInstance(ModdableItemInstance moddableItem, ItemModSlotData modSlotData)
    {
        this.parentModdableItem = moddableItem;
        this.ModSlotData = modSlotData;
    }

    public void SetMod(ItemInstance mod)
    {
        this.mod = mod;
    }

    public bool IsCompatible(ItemInstance mod)
    {
        foreach (string tag in ModSlotData.CompatibilityTags)
        {
            if (!mod.Tags.Contains(tag))
            {
                return false;
            }
        }
        return true;
    }
}
