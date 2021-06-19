using ProjectBlast.Items.Containers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IContainer
{
    UnityAction<ContainerItemInstance> OnAddItem { get; set; }
    UnityAction<ContainerItemInstance> OnRemoveItem { get; set; }

    int Width { get; }
    int Height { get; }
    int Size { get; }
    Vector2Int Dimensions { get; }

    public void Initialize(int width, int height);

    IList<ContainerItemInstance> GetItems();

    bool AddItem(ItemInstance itemInstance);

    bool AddItem(ItemInstance itemInstance, Vector2Int coordinates, bool isRotated);

    bool RemoveItem(ItemInstance item);

    bool IsCoordinatesClear(Vector2Int coordinates);

    bool IsCoordinatesClear(List<Vector2Int> coordinates);
}
