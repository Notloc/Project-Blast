using Notloc.Utility;
using ProjectBlast.Items.Containers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDragItemHoverReceiver : IGameObject
{
    void HoverItem(IDragItem dragItem, Vector2 mousePosition);
    void ClearHover();
}
