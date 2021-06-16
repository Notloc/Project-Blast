using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ModdableItemInstance : ItemInstance
{
    public IList<ItemModData> InstalledMods => installedMods.AsReadOnly();
    [SerializeField] protected List<ItemModData> installedMods = new List<ItemModData>();

    public ModdableItemBase ModdableItemBase => (ModdableItemBase)ItemBase;

    public ModdableItemInstance(ModdableItemBase itemBase) : base(itemBase) {}
}
