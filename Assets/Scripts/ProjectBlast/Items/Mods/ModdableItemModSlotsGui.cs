using ProjectBlast.Items.Containers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ModdableItemModSlotsGui : MonoBehaviour
{
    [SerializeField] ItemModSlotGui modSlotPrefab = null;
    [SerializeField] RectTransform modSlotParent = null;

    [SerializeField] TextMeshProUGUI moddableItemName = null;

    private List<ItemModSlotGui> modSlotGuis = new List<ItemModSlotGui>();
    private Dictionary<string, ItemModSlotGui> modSlotsByName = new Dictionary<string, ItemModSlotGui>();

    public void SetModdableItem(ModdableItemInstance moddableItem, ItemInstance topLevelItem, IContainer container, UnityAction<ItemModSlotInstance> onModAdded, UnityAction<ItemModSlotInstance, ItemInstance> onModRemoved)
    {
        Clear();

        moddableItemName.text = moddableItem.Name;
        foreach (ItemModSlotInstance modSlot in moddableItem.ModSlots)
        {
            ItemModSlotGui modSlotGui = Instantiate(modSlotPrefab, modSlotParent);
            modSlotGui.Initialize(modSlot, topLevelItem, container, onModAdded, onModRemoved);
            modSlotGuis.Add(modSlotGui);
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
