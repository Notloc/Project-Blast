using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBlast.Items.Draggable
{
    public interface IDraggableItemReceiver
    {
        void ReceiveDraggedItem(IDraggableItem draggedItem, Vector2 mousePosition);
        void RemoveDraggedItem(IDraggableItem draggedItem);
    }
}