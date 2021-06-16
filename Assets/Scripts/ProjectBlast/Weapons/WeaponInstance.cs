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

    public WeaponInstance(WeaponBase weaponBase, List<ItemModData> attachments = null) : base(weaponBase)
    {
        this.weaponBase = weaponBase;
        itemSize = base.BaseSize;
        SetAttachments(attachments);
    }

    public void SetAttachments(List<ItemModData> attachments)
    {
        installedMods.Clear();
        if (attachments != null)
        {
            foreach (var a in attachments)
            {
                installedMods.Add(a);
            }
        }
        CalculateSize();
    }

    private void CalculateSize()
    {
        Vector2Int modSize = Vector2Int.zero;
        foreach (ItemModData data in installedMods)
        {
            WeaponAttachmentInstance attachmentInstance = data.itemInstance as WeaponAttachmentInstance;
            if (attachmentInstance == null)
                continue;
            modSize += attachmentInstance.WeaponAttachmentBase.SizeModifier;
        }

        itemSize = modSize + base.BaseSize;
    }
}
