using ProjectBlast.Items.Containers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ModdableItemInspectGui : GenericItemInspectGui
{
    [SerializeField] RectTransform attachmentSlotsParent = null;
    [SerializeField] ModdableItemSlotsGui moddableItemSlotsPrefab = null;

    private List<ModdableItemSlotsGui> modSlotGuis = new List<ModdableItemSlotsGui>();
    private Dictionary<ItemInstance, ModdableItemSlotsGui> modSlotGuisByItems = new Dictionary<ItemInstance, ModdableItemSlotsGui>();
    private Dictionary<ItemInstance, ItemInstance> parentItemMap = new Dictionary<ItemInstance, ItemInstance>();

    public override void SetItem(ItemInstance item)
    {
        base.SetItem(item);
        Clear();

        ModdableItemInstance moddable = (ModdableItemInstance)item;
        CreateModdableItemSlotsGui(moddable, null);
    }

    private void CreateModdableItemSlotsGui(ModdableItemInstance moddableItem, ItemInstance parent)
    {
        if (parent != null)
        {
            parentItemMap.Add(moddableItem, parent);
        }

        if (moddableItem != null && moddableItem.ModdableItemBase.ModSlots.Count > 0)
        {
            ModdableItemSlotsGui modSlotsGui = Instantiate(moddableItemSlotsPrefab, attachmentSlotsParent);
            modSlotGuis.Add(modSlotsGui);
            
            modSlotsGui.SetModdableItem(moddableItem);
            modSlotsGui.OnItemAdded += OnItemAdded;
            modSlotsGui.OnItemRemoved += OnItemRemoved;

            modSlotGuisByItems.Add(moddableItem, modSlotsGui);
        }

        foreach (ItemModData data in moddableItem.InstalledMods)
        {
            if (data.itemInstance is ModdableItemInstance)
            {
                CreateModdableItemSlotsGui((ModdableItemInstance)data.itemInstance, moddableItem);
            }
        }
    }

    private void Clear()
    {
        foreach (ModdableItemSlotsGui gui in modSlotGuis)
        {
            Destroy(gui.gameObject);
        }
    }

    private void OnItemAdded(ItemInstance item, ItemInstance parent)
    {
        ModdableItemInstance moddableItem = item as ModdableItemInstance;
        if (moddableItem != null)
        {
            CreateModdableItemSlotsGui(moddableItem, parent);
        }
    }

    private void OnItemRemoved(ItemInstance item)
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
            parentItemMap.Remove(mod);
        }

    }

    private void GetChildAttachments(ItemInstance item, List<ItemInstance> outList)
    {
        outList.Add(item);
        ModdableItemInstance moddableItem = item as ModdableItemInstance;
        if (moddableItem != null)
        {
            foreach (ItemModData modData in moddableItem.InstalledMods)
            {
                GetChildAttachments(modData.itemInstance, outList);
            }
        }
    }
}
