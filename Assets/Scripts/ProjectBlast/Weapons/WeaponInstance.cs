using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponInstance : ModdableItemInstance
{
    public WeaponBase WeaponBase => weaponBase;
    [SerializeField] WeaponBase weaponBase = null;

    public override Vector2Int BaseSize => itemSize;
    private Vector2Int itemSize = Vector2Int.one;

    public WeaponInstance(WeaponBase itemBase) : base(itemBase)
    {
        weaponBase = itemBase;
        itemSize = base.BaseSize;
    }

    public void SetAttachments(List<WeaponAttachmentInstance> attachments)
    {
        installedMods.Clear();
        foreach (var a in attachments)
        {
            installedMods.Add(a);
        }
        CalculateSize();
    }

    private void CalculateSize()
    {
        Vector2Int modSize = Vector2Int.zero;
        foreach (ItemInstance item in installedMods)
        {
            WeaponAttachmentInstance attachmentInstance = item as WeaponAttachmentInstance;
            if (attachmentInstance == null)
                continue;
            modSize += attachmentInstance.WeaponAttachmentBase.SizeModifier;
        }

        itemSize = modSize + base.BaseSize;
    }
}
