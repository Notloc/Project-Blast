using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDragItemReceiver
{
    void ReceiveDraggedItem(IDragItem draggedItem, Vector2 mousePosition);
    void RemoveDraggedItem(IDragItem draggedItem);
}
