using ProjectBlast.Items.Containers;
using ProjectBlast.Items.Containers.Gui;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemModSlotGui : MonoBehaviour, IDragItemReceiver, IDragItemHoverReceiver
{
    [SerializeField] TextMeshProUGUI slotNameText = null;
    [SerializeField] ContainerItemGui containerItemGui = null;

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

    private void OnDestroy()
    {
        if (modSlot == null)
        {
            return;
        }

        ModdableItemInstance moddableItem = modSlot.parentModdableItem;
        moddableItem.OnModAdded -= OnModAdded;
        moddableItem.OnModRemoved -= OnModRemoved;
    }

    public void Initialize(ItemModSlotInstance modSlot, ModdableItemInstance topLevelItem, IContainer container, UnityAction<ItemModSlotInstance> onAddItem, UnityAction<ItemModSlotInstance, ItemInstance> onRemoveItem)
    {
        this.TopLevelModdableItem = topLevelItem;
        this.container = container;

        this.onAddItem = onAddItem;
        this.onRemoveItem = onRemoveItem;

        this.modSlot = modSlot;
        slotNameText.text = modSlot.ModSlotData.SlotName;

        ModdableItemInstance moddableItem = modSlot.parentModdableItem;
        moddableItem.OnModAdded += OnModAdded;
        moddableItem.OnModRemoved += OnModRemoved;

        containerItemGui.SetContainer(null);
    }

    public void SetItem(ItemInstance item)
    {
        this.item = item;
        containerItemGui.gameObject.SetActive(item != null);
        containerItemGui.SetContainerItem(this, item != null ? new ContainerItemEntry(item, null,  Vector2Int.zero) : default);
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









    // TODO: Hover effects
    public void HoverItem(Vector2Int itemDimensions, Vector2Int coordinates, bool isValid) {}
    public void ClearHover() {}

    public ContainerItemEntry GetContainerItem()
    {
        return new ContainerItemEntry(item, null, Vector2Int.zero);
    }

    public void HoverItem(IDragItem dragItem, Vector2 mousePosition)
    {
        throw new System.NotImplementedException();
    }

    public void ReceiveDraggedItem(IDragItem draggedItem, Vector2 mousePosition)
    {
        throw new System.NotImplementedException();
    }

    public void RemoveDraggedItem(IDragItem draggedItem)
    {
        throw new System.NotImplementedException();
    }
}
