using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemModSlotPositionData
{
    public string SlotName => slotName;
    [SerializeField] string slotName = "SLOT_NAME";

    public Transform AttachmentPoint => attachmentPoint;
    [SerializeField] Transform attachmentPoint = null;
}
