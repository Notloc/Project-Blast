using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponAttachmentInstanceHolder : ScriptableObject
{
    [SerializeField] WeaponAttachmentBase attachmentBase = null;
    [SerializeField] List<SubAttachmentData> subAttachments = null;

    public WeaponAttachmentInstance GetWeaponAttachmentInstance()
    {
        return new WeaponAttachmentInstance(attachmentBase, GetSubAttachments());
    }

    private List<ItemModData> GetSubAttachments()
    {
        List<ItemModData> modData = new List<ItemModData>();
        foreach (SubAttachmentData data in subAttachments)
        {
            WeaponAttachmentInstance attachment = data.subAttachment.GetWeaponAttachmentInstance();
            modData.Add(new ItemModData(attachment, data.slotName));
        }
        return modData;
    }

    [System.Serializable]
    private class SubAttachmentData
    {
        public WeaponAttachmentInstanceHolder subAttachment = null;
        public string slotName = "Slot";
    }
}
