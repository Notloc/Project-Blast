using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    ItemBase itemBase;
    [SerializeField] uint itemId;
    public Item(ItemBase itemBase)
    {
        this.itemBase = itemBase;
        itemId = itemBase.GetId();
    }

    public ItemBase GetBase()
    {
        return itemBase;
    }
    public GameObject GetModel()
    {
        return itemBase.GetModel();
    }
}
