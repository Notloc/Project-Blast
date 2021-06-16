using ProjectBlast.Items.Containers;
using ProjectBlast.Items.Containers.Gui;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemModSlotGui : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI slotName = null;
    [SerializeField] ContainerItemGui containerItem = null;

    public ItemModSlot ModSlot => modSlot;
    private ItemModSlot modSlot;

    private ContainerItemInstance item;

    public void SetItemModSlot(ItemModSlot modSlot)
    {
        this.modSlot = modSlot;
        slotName.text = modSlot.SlotName;
    }

    public void SetItem(ContainerItemInstance item)
    {
        this.item = item;
        containerItem.SetItemInstance(item);
    }
}
