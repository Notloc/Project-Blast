using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemModSlot
{
    [SerializeField] string slotName = "Mod Slot";
    [SerializeField] List<string> compatibilityTags = new List<string>();

    public string SlotName => slotName;
    public IList<string> CompatibilityTags => compatibilityTags.AsReadOnly();
}
