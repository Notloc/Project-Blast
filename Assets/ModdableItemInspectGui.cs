using ProjectBlast.Items.Containers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class ModdableItemInspectGui : GenericItemInspectGui
{
    [SerializeField] RectTransform attachmentSlotsParent = null;
    [SerializeField] ModdableItemModSlotsGui moddableItemSlotsPrefab = null;

    private List<ModdableItemModSlotsGui> modSlotGuis = new List<ModdableItemModSlotsGui>();
    private Dictionary<ItemInstance, ModdableItemModSlotsGui> modSlotGuisByItems = new Dictionary<ItemInstance, ModdableItemModSlotsGui>();

    private UnityAction<ItemModSlotInstance> OnModAdded;
    private UnityAction<ItemModSlotInstance, ItemInstance> OnModRemoved;

    private ModdableItemInstance topLevelModdableItem;

    private void Awake()
    {
        OnModAdded += OnModAdd;
        OnModRemoved += OnModRemove;
    }

    private void OnDestroy()
    {
        OnModAdded -= OnModAdd;
        OnModRemoved -= OnModRemoved;
    }

    public override void SetItem(ItemInstance item, IContainer container)
    {
        base.SetItem(item, container);
        Clear();

        topLevelModdableItem = (ModdableItemInstance)item;
        CreateModdableItemSlotsGui(topLevelModdableItem, topLevelModdableItem);
    }

    private void CreateModdableItemSlotsGui(ModdableItemInstance moddableItem, ModdableItemInstance topLevelModdableItem)
    {
        if (moddableItem != null && moddableItem.ModdableItemBase.ModSlots.Count > 0)
        {
            ModdableItemModSlotsGui modSlotsGui = Instantiate(moddableItemSlotsPrefab, attachmentSlotsParent);
            modSlotGuis.Add(modSlotsGui);
            
            modSlotsGui.SetModdableItem(moddableItem, topLevelModdableItem, container, OnModAdded, OnModRemoved);


            modSlotGuisByItems.Add(moddableItem, modSlotsGui);
        }

        foreach (ItemModSlotInstance data in moddableItem.ModSlots)
        {
            if (data.Mod is ModdableItemInstance)
            {
                CreateModdableItemSlotsGui((ModdableItemInstance)data.Mod, topLevelModdableItem);
            }
        }
    }

    private void Clear()
    {
        foreach (ModdableItemModSlotsGui gui in modSlotGuis)
        {
            Destroy(gui.gameObject);
        }
    }

    private void OnModAdd(ItemModSlotInstance modSlot)
    {
        ModdableItemInstance moddableItem = modSlot.Mod as ModdableItemInstance;
        if (moddableItem != null)
        {
            CreateModdableItemSlotsGui(moddableItem, topLevelModdableItem);
        }
    }

    private void OnModRemove(ItemModSlotInstance modSlot, ItemInstance item)
    {

        List<ItemInstance> effectedMods = new List<ItemInstance>();
        GetChildAttachments(item, effectedMods);

        foreach (ItemInstance mod in effectedMods)
        {
            if (modSlotGuisByItems.ContainsKey(mod))
            {
                var slot = modSlotGuisByItems[mod];
                modSlotGuisByItems.Remove(mod);
                modSlotGuis.Remove(slot);
                Destroy(slot.gameObject); // TODO: Pool instead
            }
        }
    }

    private void GetChildAttachments(ItemInstance item, List<ItemInstance> outList)
    {
        outList.Add(item);
        ModdableItemInstance moddableItem = item as ModdableItemInstance;
        if (moddableItem != null)
        {
            foreach (ItemModSlotInstance modData in moddableItem.ModSlots)
            {
                GetChildAttachments(modData.Mod, outList);
            }
        }
    }
}
