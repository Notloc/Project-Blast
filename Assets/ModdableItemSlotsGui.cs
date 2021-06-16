using ProjectBlast.Items.Containers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class ModdableItemSlotsGui : MonoBehaviour
{
    [SerializeField] ItemModSlotGui modSlotPrefab = null;
    [SerializeField] RectTransform modSlotParent = null;

    [SerializeField] TextMeshProUGUI moddableItemName = null;

    private ModdableItemInstance moddableItem;

    private List<ItemModSlotGui> modSlotGuis = new List<ItemModSlotGui>();
    private Dictionary<string, ItemModSlotGui> modSlotsByName = new Dictionary<string, ItemModSlotGui>();

    public void SetModdableItem(ModdableItemInstance moddableItem)
    {
        Clear();

        this.moddableItem = moddableItem;

        moddableItemName.text = moddableItem.Name;

        foreach (ItemModSlot modSlot in moddableItem.ModdableItemBase.ModSlots)
        {
            var modSlotGui = Instantiate(modSlotPrefab, modSlotParent);
            modSlotGui.SetItemModSlot(modSlot);
            modSlotGuis.Add(modSlotGui);
        }
        modSlotsByName = Util.Dictionary(modSlotGuis, slotGui => slotGui.ModSlot.SlotName);

        foreach (var modData in moddableItem.InstalledMods)
        {
            modSlotsByName[modData.modSlotName].SetItem(new ContainerItemInstance(modData.itemInstance, Vector2Int.zero));
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
}
