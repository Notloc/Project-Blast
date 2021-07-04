using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectBlast.Items.Containers
{
    public interface IContainer
    {
        UnityAction<ContainerEntry> OnAddItem { get; set; }
        UnityAction<ContainerEntry> OnRemoveItem { get; set; }
        UnityAction<ContainerEntry> OnUpdateItem { get; set; }

        int Width { get; }
        int Height { get; }
        int Size { get; }
        Vector2Int Dimensions { get; }

        IList<ContainerEntry> GetItems();

        bool AddItem(ItemInstance itemInstance);

        bool AddItem(ItemInstance itemInstance, Vector2Int coordinates, bool isRotated);

        bool MoveItem(ItemInstance itemInstance, Vector2Int coordinates, bool isRotated);

        bool RemoveItem(ItemInstance item);

        bool DoesItemFit(ItemInstance item, Vector2Int coordinates, bool isRotated);

        bool IsCoordinatesClear(Vector2Int coordinates);

        bool IsCoordinatesClear(List<Vector2Int> coordinates);

        bool WillResizedItemFit(ItemInstance item, ItemInstance other, Vector2Int sizeMod);
    }
}