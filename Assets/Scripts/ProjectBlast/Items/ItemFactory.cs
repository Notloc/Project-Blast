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
        Item item = ScriptableObject.CreateInstance("Item") as Item;
        item.Init(itemBase);
        return item;
    }

}
