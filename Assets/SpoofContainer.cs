using ProjectBlast.Items.Containers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// A spoof container that doesn't care
public class SpoofContainer : MonoBehaviour, IContainer
{
    public UnityAction<ContainerItemInstance> OnAddItem { get; set; }
    public UnityAction<ContainerItemInstance> OnRemoveItem { get; set; }

    public int Width => 10;
    public int Height => 10;
    public int Size => 100;
    public Vector2Int Dimensions => Vector2Int.one * 10;

    public void Initialize(int width, int height) {}

    public bool AddItem(ItemInstance itemInstance)
    {
        ContainerItemInstance containerItem = new ContainerItemInstance(itemInstance, Vector2Int.zero);
        OnAddItem?.Invoke(containerItem);
        return true;
    }

    public bool AddItem(ItemInstance itemInstance, Vector2Int coordinates, bool isRotated)
    {
        ContainerItemInstance containerItem = new ContainerItemInstance(itemInstance, coordinates, isRotated);
        OnAddItem?.Invoke(containerItem);
        return true;
    }

    public bool RemoveItem(ItemInstance itemInstance)
    {
        ContainerItemInstance containerItem = new ContainerItemInstance(itemInstance, Vector2Int.zero);
        OnRemoveItem?.Invoke(containerItem);
        return true;
    }

    public IList<ContainerItemInstance> GetItems()
    {
        return null;
    }

    public bool IsCoordinatesClear(Vector2Int coordinates)
    {
        return true;
    }

    public bool IsCoordinatesClear(List<Vector2Int> coordinates)
    {
        return true;
    }

    
}
