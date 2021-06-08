using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    public static ItemFactory Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Item CreateItem(ItemBase itemBase)
    {
        if (itemBase as EquipmentBase)
        {
            return CreateEquipment(itemBase);
        }
        else
        {
            return CreateRegularItem(itemBase);
        }
    }

    private Item CreateRegularItem(ItemBase itemBase)
    {
        Item item = ScriptableObject.CreateInstance("Item") as Item;
        item.Init(itemBase);
        return item;
    }
    private Item CreateEquipment(ItemBase itemBase)
    {
        if (itemBase as GunBase)
        {
            GunBase gunBase = (GunBase)itemBase;
            GunItem gunItem = ScriptableObject.CreateInstance("GunItem") as GunItem;
            gunItem.Init(gunBase);
            return gunItem;
        }
        throw new NotImplementedException();
    }
}
