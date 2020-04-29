using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpewer : MonoBehaviour
{
    [Space]
    [SerializeField] ItemEntity itemPrefab = null;   
    [SerializeField] Transform spawnPoint = null;
    [SerializeField] List<ItemBase> items = new List<ItemBase>();

    [SerializeField] float delay = 0.2f;
    [SerializeField] float numberOfItems = 25f;

    private float lastItemTime = -100f;
    private int itemsSpawned = 0;
    private void Update()
    {
        if (itemsSpawned < numberOfItems)
            return;
        if (Time.time > lastItemTime)
        {
            SpawnItem();
            lastItemTime = Time.time + delay;
            itemsSpawned++;
        }
    }

    private void SpawnItem()
    {
        ItemBase randomBase = items[Random.Range(0, items.Count)];
        Item newItem = new Item(randomBase);
        ItemEntityFactory.CreateItemEntity(itemPrefab, newItem, spawnPoint.position);
    }
}
