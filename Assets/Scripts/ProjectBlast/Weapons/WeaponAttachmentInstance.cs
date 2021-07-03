using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponAttachmentInstance : ModdableItemInstance
{
    public WeaponAttachmentBase WeaponAttachmentBase { get; set; }
    
    public Vector2Int SizeMod => sizeMod;
    private Vector2Int sizeMod = Vector2Int.zero;

    public WeaponAttachmentInstance(WeaponAttachmentBase itemBase) : base(itemBase)
    {
        WeaponAttachmentBase = itemBase;
        sizeMod = itemBase.SizeModifier;

        OnModAdded += RecalculateSizeMod;
        OnModRemoved += RecalculateSizeMod;
    }

    ~WeaponAttachmentInstance()
    {
        OnModAdded -= RecalculateSizeMod;
        OnModRemoved -= RecalculateSizeMod;
    }

    private void RecalculateSizeMod(ItemModSlotInstance modSlot)
    {
        WeaponAttachmentInstance attachment = modSlot.Mod as WeaponAttachmentInstance;
        if (attachment != null)
        {
            sizeMod += attachment.WeaponAttachmentBase.SizeModifier;
        }
    }

    private void RecalculateSizeMod(ItemModSlotInstance modSlot, ItemInstance itemRemoved)
    {
        WeaponAttachmentInstance attachment = itemRemoved as WeaponAttachmentInstance;
        if (attachment != null)
        {
            sizeMod -= attachment.WeaponAttachmentBase.SizeModifier;
        }
    }
}
