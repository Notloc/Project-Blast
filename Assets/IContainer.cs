using ProjectBlast.Items.Containers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IContainer
{
    UnityAction<ContainerItemEntry> OnAddItem { get; set; }
    UnityAction<ContainerItemEntry> OnRemoveItem { get; set; }
    UnityAction<ContainerItemEntry> OnUpdateItem { get; set; }

    int Width { get; }
    int Height { get; }
    int Size { get; }
    Vector2Int Dimensions { get; }

    IList<ContainerItemEntry> GetItems();

    bool AddItem(ItemInstance itemInstance);

    bool AddItem(ItemInstance itemInstance, Vector2Int coordinates, bool isRotated);

    bool MoveItem(ItemInstance itemInstance, Vector2Int coordinates, bool isRotated);

    bool RemoveItem(ItemInstance item);

    bool DoesItemFit(ItemInstance item, Vector2Int coordinates, bool isRotated);

    bool IsCoordinatesClear(Vector2Int coordinates);

    bool IsCoordinatesClear(List<Vector2Int> coordinates);

    bool WillResizedItemFit(ItemInstance item, Vector2Int sizeMod);
}
