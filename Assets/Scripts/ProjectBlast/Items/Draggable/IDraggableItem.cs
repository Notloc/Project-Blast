using Notloc.Utility;
using ProjectBlast.Items.Containers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBlast.Items.Draggable
{
    public interface IDraggableItem
    {
        ContainerEntry ContainerEntry { get; }
        IDraggableItemReceiver ParentDragItemReceiver { get; }
        RectTransform RectTransform { get; }
    }
}