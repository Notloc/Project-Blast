using ProjectBlast.Items.Containers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDragItem
{
    ContainerItemEntry ContainerItem { get; }
    IDragItemReceiver ParentDragItemReceiver { get; }
    RectTransform RectTransform { get; }
}
