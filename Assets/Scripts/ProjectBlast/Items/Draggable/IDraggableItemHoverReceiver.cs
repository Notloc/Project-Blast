using Notloc.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBlast.Items.Draggable
{
    public interface IDraggableItemHoverReceiver
    {
        void HoverItem(IDraggableItem dragItem, Vector2 mousePosition);
        void ClearHover();
    }
}