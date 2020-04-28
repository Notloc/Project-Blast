using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemEntityFactory
{
    private static ItemDatabase ItemDB { get { return ItemDatabase.Instance; } }

    public static ItemEntity CreateItemEntity(ItemEntity prefab, Item item, Vector3 position)
    {
        ItemEntity newItem = Object.Instantiate(prefab, position, Quaternion.identity);
        newItem.Initialize(item);
        return newItem;
    }
}
