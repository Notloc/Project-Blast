using ProjectBlast.Items.Containers.Gui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModSlotContainerItemGui : ContainerItemGui
{
    protected override void Resize()
    {
        rect.sizeDelta = Vector2Int.one * ContainerSlotGui.SLOT_SIZE_PIXELS;
    }
}
