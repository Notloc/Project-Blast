using ProjectBlast.Items.Containers.Gui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModSlotContainerItemGui : ContainerEntryGui
{
    protected override void Resize()
    {
        ((RectTransform)transform).sizeDelta = Vector2Int.one * ContainerSlotGui.SLOT_SIZE_PIXELS;
    }
}
