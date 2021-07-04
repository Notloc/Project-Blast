using ProjectBlast.Items.Containers;
using ProjectBlast.Items.Containers.Gui;
using ProjectBlast.Items.Draggable;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemModSlotGui : MonoBehaviour, IDraggableItemReceiver, IDraggableItemHoverReceiver
{
    [SerializeField] TextMeshProUGUI slotNameText = null;
    [SerializeField] ContainerEntryGui containerItemGui = null;

    [SerializeField] Image hoverHighlight = null;
    [SerializeField] Color compatibileHighlight = Color.green;
    [SerializeField] Color incompatibileHighlight = Color.red;

    public ItemModSlotInstance ModSlot => modSlot;
    private ItemModSlotInstance modSlot;

    public ItemInstance Item => item;
    private ItemInstance item;

    private IContainer container;
    public ModdableItemInstance TopLevelModdableItem { get; private set; }

    private UnityAction<ItemModSlotInstance> onAddItem;
    private UnityAction<ItemModSlotInstance, ItemInstance> onRemoveItem;

    private void Awake()
    {
        containerItemGui.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        SetModSlot(null);
    }

    public void Initialize(ItemModSlotInstance modSlot, ItemInstance topLevelItem, IContainer container, UnityAction<ItemModSlotInstance> onAddItem, UnityAction<ItemModSlotInstance, ItemInstance> onRemoveItem)
    {
        this.TopLevelModdableItem = (ModdableItemInstance)topLevelItem;
        this.container = container;

        this.onAddItem = onAddItem;
        this.onRemoveItem = onRemoveItem;

        SetModSlot(modSlot);

        slotNameText.text = modSlot.ModSlotData.SlotName;
        containerItemGui.SetContainer(null);
        SetItem(modSlot.Mod);
    }

    private void SetModSlot(ItemModSlotInstance newModSlot)
    {
        ModdableItemInstance moddableItem;
        if (modSlot != null)
        {
            moddableItem = modSlot.parentModdableItem;
            moddableItem.OnModAdded -= OnModAdded;
            moddableItem.OnModRemoved -= OnModRemoved;
        }

        this.modSlot = newModSlot;
        if (newModSlot != null)
        {
            moddableItem = newModSlot.parentModdableItem;
            moddableItem.OnModAdded += OnModAdded;
            moddableItem.OnModRemoved += OnModRemoved;
        }
    }

    public void SetItem(ItemInstance item)
    {
        this.item = item;
        containerItemGui.gameObject.SetActive(item != null);
        if (item != null)
        {
            containerItemGui.SetContainerItem(this, new ContainerEntry(item, null,  Vector2Int.zero));
        }
    }

    private void OnModAdded(ItemModSlotInstance modSlot)
    {
        if (this.modSlot == modSlot)
        {
            SetItem(modSlot.Mod);
            onAddItem?.Invoke(modSlot);
        }
    }

    private void OnModRemoved(ItemModSlotInstance modSlot, ItemInstance removedItem)
    {
        if (this.modSlot == modSlot)
        {
            SetItem(null);
            onRemoveItem?.Invoke(modSlot, removedItem);
        }
    }

    public void HoverItem(IDraggableItem dragItem, Vector2 mousePosition)
    {
        Color highlightColor = modSlot.IsCompatible(dragItem.ContainerEntry.Item) ? compatibileHighlight : incompatibileHighlight;
        hoverHighlight.enabled = true;
        hoverHighlight.color = highlightColor;
    }
    public void ClearHover()
    {
        hoverHighlight.enabled = false;
    }

    public ContainerEntry GetContainerItem()
    {
        return new ContainerEntry(item, null, Vector2Int.zero);
    }

    

    public void ReceiveDraggedItem(IDraggableItem dragItem, Vector2 mousePosition)
    {
        ContainerEntry itemEntry = dragItem.ContainerEntry;

        if (!modSlot.IsCompatible(itemEntry.Item))
        {
            return;
        }

        if (!ContainerService.WillModdedWeaponFit(TopLevelModdableItem, itemEntry.Item, container))
        {
            return;
        }

        dragItem.ParentDragItemReceiver.RemoveDraggedItem(dragItem);
        modSlot.parentModdableItem.AddMod(new ItemModData(itemEntry.Item, modSlot.ModSlotData.SlotName));
    }

    public void RemoveDraggedItem(IDraggableItem draggedItem)
    {
        modSlot.parentModdableItem.RemoveMod(draggedItem.ContainerEntry.Item);
    }
}
