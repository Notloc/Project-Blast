using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponAttachmentInstance : ModdableItemInstance
{
    public WeaponAttachmentBase WeaponAttachmentBase => (WeaponAttachmentBase)ItemBase;
    public WeaponAttachmentInstance(WeaponAttachmentBase itemBase, List<ItemModData> attachments = null) : base(itemBase, attachments) {}
}
