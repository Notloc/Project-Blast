using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WeaponItemInspectGui : GenericItemInspectGui
{
    [SerializeField] RectTransform attachmentSlotsParent = null;
    [SerializeField] ModdableItemSlotsGui moddableItemSlotsPrefab = null;

    private List<ModdableItemSlotsGui> modSlotGuis = new List<ModdableItemSlotsGui>();

    public override void SetItem(ItemInstance item)
    {
        base.SetItem(item);
        Clear();

        WeaponInstance weapon = (WeaponInstance)item;
        CreateModdableItemSlotsGui(weapon);
    }

    private void CreateModdableItemSlotsGui(ModdableItemInstance moddableItem)
    {
        if (moddableItem != null && moddableItem.ModdableItemBase.ModSlots.Count > 0)
        {
            ModdableItemSlotsGui modSlotsGui = Instantiate(moddableItemSlotsPrefab, attachmentSlotsParent);
            modSlotGuis.Add(modSlotsGui);
            modSlotsGui.SetModdableItem(moddableItem);
        }

        foreach (ItemModData data in moddableItem.InstalledMods)
        {
            if (data.itemInstance is ModdableItemInstance)
            {
                CreateModdableItemSlotsGui((ModdableItemInstance)data.itemInstance);
            }
        }
    }

    private void Clear()
    {
        foreach (ModdableItemSlotsGui gui in modSlotGuis)
        {
            Destroy(gui.gameObject);
        }
    }
}
