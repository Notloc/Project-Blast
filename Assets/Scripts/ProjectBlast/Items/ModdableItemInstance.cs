using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class ModdableItemInstance : ItemInstance
{
    public ModdableItemBase ModdableItemBase => (ModdableItemBase)ItemBase;

    public UnityAction<ItemModSlotInstance> OnModAdded;
    public UnityAction<ItemModSlotInstance, ItemInstance> OnModRemoved;

    public IList<ItemModSlotInstance> ModSlots => modSlots.AsReadOnly();
    [SerializeField] private List<ItemModSlotInstance> modSlots = new List<ItemModSlotInstance>();
    private Dictionary<string, ItemModSlotInstance> modSlotsByName;

    public override Vector2Int Size => itemSize;
    private Vector2Int itemSize = Vector2Int.one;

    public ModdableItemInstance(ModdableItemBase itemBase) : base(itemBase)
    {
        itemSize = base.Size;
        CreateModSlots();
    }

    private void CreateModSlots()
    {
        foreach (ItemModSlotData modSlot in ModdableItemBase.ModSlots)
        {
            modSlots.Add(new ItemModSlotInstance(this, modSlot));
        }
        modSlotsByName = Util.Dictionary(modSlots, slot => slot.ModSlotData.SlotName);
    }

    public void AddMod(ItemModData modData)
    {
        if (!modSlotsByName.ContainsKey(modData.slotName))
        {
            return;
        }

        ItemInstance modToAdd = modData.mod;
        ItemModSlotInstance modSlot = modSlotsByName[modData.slotName];
        if (modSlot.Mod == null && modToAdd != null)
        {
            modSlot.SetMod(modToAdd);

            ModdableItemInstance moddableItem = modToAdd as ModdableItemInstance;
            if (moddableItem != null)
            {
                moddableItem.OnModAdded += OnModAdded;
            }

            OnModAdded?.Invoke(modSlot);
            CalculateSize();
            OnItemUpdated?.Invoke(this);
        }
    }

    public void RemoveMod(ItemInstance item)
    {
        foreach (ItemModSlotInstance modSlot in modSlots)
        {
            if (modSlot.Mod == item)
            {
                ModdableItemInstance moddableItem = modSlot.Mod as ModdableItemInstance;
                if (moddableItem != null)
                {
                    moddableItem.OnModAdded -= OnModAdded;
                }

                modSlot.SetMod(null);
                OnModRemoved?.Invoke(modSlot, item);
                CalculateSize();
                OnItemUpdated?.Invoke(this);
            }
        }
    }

    private void CalculateSize()
    {
        Vector2Int modSize = Vector2Int.zero;
        foreach (ItemModSlotInstance modSlot in ModSlots)
        {
            WeaponAttachmentInstance attachmentInstance = modSlot.Mod as WeaponAttachmentInstance;
            if (attachmentInstance == null)
                continue;
            modSize += attachmentInstance.WeaponAttachmentBase.SizeModifier;
        }

        itemSize = modSize + base.Size;
    }

    public void SetModData(List<ItemModData> modData)
    {
        foreach (ItemModData mod in modData)
        {
            AddMod(mod);
        }
    }

    public void TriggerUpdate()
    {
        OnItemUpdated?.Invoke(this);
    }
}
