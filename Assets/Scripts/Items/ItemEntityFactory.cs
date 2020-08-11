using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntityFactory : MonoBehaviour
{
    public static ItemEntityFactory Instance { get; private set; }
    [SerializeField] ItemEntity itemEntityPrefab = null;

    private void Awake()
    {
        Instance = this;
    }

    public ItemEntity CreateItemEntity(Item item, Vector3 position)
    {
        ItemEntity newItem = Object.Instantiate(itemEntityPrefab, position, Quaternion.identity);
        newItem.Init(item);
        return newItem;
    }
}
