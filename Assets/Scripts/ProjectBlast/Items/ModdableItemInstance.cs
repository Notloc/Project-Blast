using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ModdableItemInstance : ItemInstance
{
    public IList<ItemInstance> InstalledMods => installedMods.AsReadOnly();
    [SerializeField] protected List<ItemInstance> installedMods = new List<ItemInstance>();

    public ModdableItemInstance(ModdableItemBase itemBase) : base(itemBase) {}
}
