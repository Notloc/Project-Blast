using ProjectBlast.Items.Containers;
using ProjectBlast.Items.Containers.Gui;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemModSlotGui : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI slotNameText = null;
    [SerializeField] ContainerItemGui containerItem = null;

    public UnityAction<ItemInstance, string> OnAddedItem;
    public UnityAction<ItemInstance, string> OnRemovedItem;

    public ItemModSlot ModSlot => modSlot;
    private ItemModSlot modSlot;

    public ItemInstance Item => item;
    private ItemInstance item;

    [SerializeField] SpoofContainer container = null;

    private void Awake()
    {
        containerItem.gameObject.SetActive(false);
    }

    public void Initialize(ItemModSlot modSlot)
    {
        container.OnAddItem += OnAddItem;
        container.OnRemoveItem += OnRemoveItem;

        this.modSlot = modSlot;
        slotNameText.text = modSlot.SlotName;
        containerItem.SetContainer(container);
    }

    public void SetItem(ItemInstance item)
    {
        this.item = item;
        containerItem.gameObject.SetActive(item != null);
        containerItem.SetItemInstance(item != null ? new ContainerItemInstance(item, Vector2Int.zero) : null);
    }

    private void OnAddItem(ContainerItemInstance containerItem)
    {
        if (this.item == null)
        {
            SetItem(containerItem.Item);
            OnAddedItem?.Invoke(containerItem.Item, modSlot.SlotName);
        }
    }

    private void OnRemoveItem(ContainerItemInstance containerItem)
    {
        if (containerItem.Item == item)
        {
            OnRemovedItem?.Invoke(item, modSlot.SlotName);
            SetItem(null);
        }
    }
}
