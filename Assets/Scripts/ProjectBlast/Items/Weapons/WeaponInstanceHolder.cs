using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponInstanceHolder : ScriptableObject
{
    [SerializeField] WeaponBase weaponBase = null;
    [SerializeField] List<WeaponAttachmentData> attachments = null;

    public WeaponInstance CreateWeaponInstance()
    {
        WeaponInstance weapon = new WeaponInstance(weaponBase);
        weapon.SetModData(GetItemModData());
        return weapon;
    }

    private List<ItemModData> GetItemModData()
    {
        List<ItemModData> modData = new List<ItemModData>();
        foreach (WeaponAttachmentData attachmentData in attachments)
        {
            modData.Add(new ItemModData(attachmentData.attachment.GetWeaponAttachmentInstance(), attachmentData.slotName));
        }
        return modData;
    }

    [System.Serializable]
    private class WeaponAttachmentData
    {
        public WeaponAttachmentInstanceHolder attachment = null;
        public string slotName = "Slot";
    }
}
