using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponAttachmentInstance : ModdableItemInstance
{
    public WeaponAttachmentBase WeaponAttachmentBase => weaponAttachmentBase;
    [SerializeField] WeaponAttachmentBase weaponAttachmentBase;

    public string SlotName => weaponAttachmentBase.SlotName;

    public WeaponAttachmentInstance(WeaponAttachmentBase itemBase) : base(itemBase)
    {
        this.weaponAttachmentBase = itemBase;
    }
}
