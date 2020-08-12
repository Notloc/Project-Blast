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
    public ContainerType ContainerType { get { return ContainerType.BASIC; } }

    public ItemDictionary Items { get { return items; } }
    [SerializeField] protected ItemDictionary items;

    public void Init(ContainerContents containerCont)
    {
        items = new ItemDictionary();

       var contents = containerCont.GetContents();

        List<ContainerItem> itemCounts = new List<ContainerItem>();
        foreach(ItemBaseCount baseCount in contents)
        {
            Item item = Game.Instance.Factories.ItemFactory.CreateItem(baseCount.itemBase);
            itemCounts.Add(new ContainerItem() { item = item, count = baseCount.count });
        }

        Add(itemCounts);
    }

    protected virtual bool CanAdd(List<ContainerItem> items) { return true; }
    protected virtual bool CanRemove(List<ContainerItem> itemsToRemove)
    {
        foreach (var removeItem in itemsToRemove)
        {
            ScriptableItem key = removeItem.Key;

            // Not in container
            if (!Items.ContainsKey(key))
                return false;

            ContainerItem cItem;
            Items.TryGetValue(key, out cItem);
            // Not enough in container
            if (cItem.count < removeItem.count)
                return false;
        }

        return true;
    }

    public bool Add(Item item, uint count = 1)
    {
        return Add(new List<ContainerItem>() {
            new ContainerItem() {
                item = item,
                count = count
            }
        });
    }

    public bool Add(List<ContainerItem> itemsToAdd)
    {
        if (!CanAdd(itemsToAdd))
            return false;

        foreach(var addItem in itemsToAdd)
        {
            ScriptableItem key = addItem.Key;

            // Add to existing stack
            if (!addItem.item.IsUnique && Items.ContainsKey(key))
            {
                ContainerItem cItem;
                Items.TryGetValue(key, out cItem);
                cItem.count += addItem.count;
                Items[key] = cItem;
            }
            else
            { // Add a new stack
                Items.Add(addItem.Key, addItem);
            }
        }
        return true;
    }


    public bool Remove(ContainerItem itemToRemove)
    {
        return Remove(new List<ContainerItem>() { itemToRemove });
    }
    public bool Remove(List<ContainerItem> itemsToRemove)
    {
        if (!CanRemove(itemsToRemove))
            return false;

        foreach(var removeItem in itemsToRemove)
        {
            ScriptableItem key = removeItem.Key;

            ContainerItem cItem;
            Items.TryGetValue(key, out cItem);
            cItem.count -= removeItem.count;

            if (cItem.count == 0)
                Items.Remove(key);
            else
                Items[key] = cItem;
        }
        return true;
    }
}