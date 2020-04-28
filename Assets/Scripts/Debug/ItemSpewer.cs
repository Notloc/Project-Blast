using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpewer : MonoBehaviour
{
    [SerializeField] ItemEntity itemPrefab;
    [Space]
    [SerializeField] Transform spawnPoint;
    [SerializeField] List<ItemBase> items;

    [SerializeField] float delay = 0.2f;
    [SerializeField] float numberOfItems = 25f;

    private float lastItemTime = -100f;
    private void Update()
    {
        if (Time.time > lastItemTime)
        {
            SpawnItem();
            lastItemTime = Time.time + delay;
        }
    }

    private void SpawnItem()
    {
        ItemBase randomBase = items[Random.Range(0, items.Count)];
        Item newItem = new Item(randomBase);
        ItemEntityFactory.CreateItemEntity(itemPrefab, newItem, spawnPoint.position);
    }
}
