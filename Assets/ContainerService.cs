using ProjectBlast.Items.Containers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ContainerService
{



    private static bool AddToModdableItem(ItemInstance modToAdd, ItemModSlotGui modSlotGui)
    {
        ItemModSlotInstance modSlot = modSlotGui.ModSlot;
        ModdableItemInstance moddableItem = modSlot.parentModdableItem;
        ModdableItemInstance topLevelModdableItem = modSlotGui.TopLevelModdableItem;
        IContainer topLevelContainer = null;// = modSlotGui.GetContainer();

        if (!modSlot.IsCompatible(modToAdd))
        {
            return false;
        }

        // The item we are modding is in a container
        if (topLevelContainer != null)
        {
            // Verify that the modded item will still fit.

            Vector2Int sizeMod = Vector2Int.zero;
            if (modToAdd is WeaponAttachmentInstance)
            {
                sizeMod = ((WeaponAttachmentInstance)modToAdd).SizeMod;
            }

            if (!topLevelContainer.WillResizedItemFit(topLevelModdableItem, sizeMod))
            {
                return false;
            }
        }

        string slotName = modSlot.ModSlotData.SlotName;
        moddableItem.AddMod(new ItemModData(modToAdd, slotName));

        return true;
    }


    private static void RemoveFromModdableItem(ItemInstance modToRemove, ItemModSlotGui modSlotGui)
    {
        ItemModSlotInstance modSlot = modSlotGui.ModSlot;
        ModdableItemInstance moddableItem = modSlot.parentModdableItem;
        moddableItem.RemoveMod(modToRemove);
    }
}
