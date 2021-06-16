using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemModSlot
{
    public string SlotName => slotName;
    [SerializeField] string slotName = "Mod Slot";

    public IList<string> CompatibilityTags => compatibilityTags.AsReadOnly();
    [SerializeField] List<string> compatibilityTags = new List<string>();
}
