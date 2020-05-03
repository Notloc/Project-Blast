using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

[Serializable]
public class Container : IContainer
{
    public Container()
    {
        items = new ItemDictionary();
    }

    public ItemDictionary Items { get { return items; } }
    [SerializeField] protected ItemDictionary items;

    public void Init(ContainerContents containerCont)
    {
        items = new ItemDictionary();

        var contents = containerCont.GetContents();

        List<ItemCount> itemCounts = new List<ItemCount>();
        foreach(ItemBaseCount baseCount in contents)
        {
            Item item = Game.Instance.Factories.ItemFactory.CreateItem(baseCount.itemBase);
            itemCounts.Add(new ItemCount() { item = item, count = baseCount.count });
        }

        Add(itemCounts);
    }

    protected virtual bool CanAdd(List<ItemCount> items) { return true; }
    protected virtual bool CanRemove(List<ItemCount> itemsToRemove)
    {
        foreach (var itemC in itemsToRemove)
        {
            ScriptableItem key = itemC.Key;

            // Not in container
            if (!Items.ContainsKey(key))
                return false;

            ItemCount itemCount;
            Items.TryGetValue(key, out itemCount);
            // Not enough in container
            if (itemCount.count < itemC.count)
                return false;
        }

        return true;
    }

    public bool Add(List<ItemCount> itemsToAdd)
    {
        if (!CanAdd(itemsToAdd))
            return false;

        foreach(var itemC in itemsToAdd)
        {
            ScriptableItem key = itemC.Key;

            // Add to existing stack
            if (!itemC.item.IsUnique && Items.ContainsKey(key))
            {
                ItemCount count;
                Items.TryGetValue(key, out count);
                count.count += itemC.count;
                Items[key] = count;
            }
            else
            { // Add a new stack
                Items.Add(itemC.Key, itemC);
            }
        }
        return true;
    }

    public bool Remove(List<ItemCount> itemsToRemove)
    {
        if (!CanRemove(itemsToRemove))
            return false;

        foreach(var itemC in itemsToRemove)
        {
            ScriptableItem key = itemC.Key;

            ItemCount itemCount;
            Items.TryGetValue(key, out itemCount);
            itemCount.count -= itemC.count;

            if (itemCount.count == 0)
                Items.Remove(key);
            else
                Items[key] = itemCount;
        }
        return true;
    }
}