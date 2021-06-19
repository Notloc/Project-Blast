using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class ModdableItemInstance : ItemInstance
{
    public ModdableItemBase ModdableItemBase => (ModdableItemBase)ItemBase;

    public UnityAction<ItemModData, ItemInstance> OnModAdded;
    public UnityAction<ItemInstance> OnModRemoved;

    public IList<ItemModData> InstalledMods => installedMods.AsReadOnly();
    [SerializeField] private List<ItemModData> installedMods = new List<ItemModData>();
    private Dictionary<ItemInstance, ItemModData> installedModsCache = new Dictionary<ItemInstance, ItemModData>();

    public override Vector2Int BaseSize => itemSize;
    private Vector2Int itemSize = Vector2Int.one;

    public ModdableItemInstance(ModdableItemBase itemBase, List<ItemModData> mods = null) : base(itemBase)
    {
        itemSize = base.BaseSize;
        SetMods(mods);
    }

    public void AddMod(ItemModData modData, ItemInstance parent)
    {
        installedMods.Add(modData);
        installedModsCache.Add(modData.itemInstance, modData);
        CalculateSize();
        OnModAdded?.Invoke(modData, parent);
    }

    public void RemoveMod(ItemInstance item)
    {
        if (installedModsCache.ContainsKey(item))
        {
            ItemModData modData = installedModsCache[item];
            installedMods.Remove(modData);
            installedModsCache.Remove(item);
            CalculateSize();
            OnModRemoved?.Invoke(item);
        }
    }

    public void SetMods(List<ItemModData> mods)
    {
        installedMods.Clear();
        installedModsCache.Clear();
        if (mods != null)
        {
            foreach (ItemModData mod in mods)
            {
                installedMods.Add(mod);
                installedModsCache.Add(mod.itemInstance, mod);
            }
        }
        CalculateSize();
    }

    private void CalculateSize()
    {
        Vector2Int modSize = Vector2Int.zero;
        foreach (ItemModData data in InstalledMods)
        {
            WeaponAttachmentInstance attachmentInstance = data.itemInstance as WeaponAttachmentInstance;
            if (attachmentInstance == null)
                continue;
            modSize += attachmentInstance.WeaponAttachmentBase.SizeModifier;
        }

        itemSize = modSize + base.BaseSize;
    }
}
