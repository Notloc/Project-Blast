using ProjectBlast.Items.Containers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ModdableItemSlotsGui : MonoBehaviour
{
    [SerializeField] ItemModSlotGui modSlotPrefab = null;
    [SerializeField] RectTransform modSlotParent = null;

    [SerializeField] TextMeshProUGUI moddableItemName = null; 

    private ModdableItemInstance moddableItem;

    private List<ItemModSlotGui> modSlotGuis = new List<ItemModSlotGui>();
    private Dictionary<string, ItemModSlotGui> modSlotsByName = new Dictionary<string, ItemModSlotGui>();
    private Dictionary<ItemInstance, ItemModSlotGui> modSlotsByItem = new Dictionary<ItemInstance, ItemModSlotGui>();

    public UnityAction<ItemInstance, ItemInstance> OnItemAdded; // added item / parent item
    public UnityAction<ItemInstance> OnItemRemoved; // removed item

    public void SetModdableItem(ModdableItemInstance moddableItem)
    {
        Clear();

        this.moddableItem = moddableItem;

        moddableItemName.text = moddableItem.Name;

        foreach (ItemModSlot modSlot in moddableItem.ModdableItemBase.ModSlots)
        {
            ItemModSlotGui modSlotGui = Instantiate(modSlotPrefab, modSlotParent);
            modSlotGui.Initialize(modSlot);
            modSlotGui.OnAddedItem += OnAddedItem;
            modSlotGui.OnRemovedItem += OnRemovedItem;

            modSlotGuis.Add(modSlotGui);
        }
        modSlotsByName = Util.Dictionary(modSlotGuis, slotGui => slotGui.ModSlot.SlotName);

        modSlotsByItem.Clear();
        foreach (var modData in moddableItem.InstalledMods)
        {
            ItemModSlotGui modSlot = modSlotsByName[modData.modSlotName];
            modSlot.SetItem(modData.itemInstance);
            modSlotsByItem.Add(modData.itemInstance, modSlot);   
        }
    }

    private void Clear()
    {
        modSlotsByName.Clear();
        foreach (ItemModSlotGui modSlotGui in modSlotGuis)
        {
            Destroy(modSlotGui);
        }
        modSlotGuis.Clear();
    }

    private void OnAddedItem(ItemInstance item, string slotName)
    {
        moddableItem.AddMod(new ItemModData(item, slotName), moddableItem);
        OnItemAdded?.Invoke(item, moddableItem);
    }

    private void OnRemovedItem(ItemInstance item, string slotName)
    {
        moddableItem.RemoveMod(item);
        OnItemRemoved?.Invoke(item);
    }
}
