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
            return CreateEquipment(itemBase as EquipmentBase);
        else
            return _CreateItem(itemBase);

    }

    private Item _CreateItem(ItemBase itemBase)
    {
        Item item = ScriptableObject.CreateInstance("Item") as Item;
        item.Init(itemBase);
        return item;
    }

    private Equipment CreateEquipment(EquipmentBase equipmentBase)
    {
        Equipment equipment = ScriptableObject.CreateInstance("Equipment") as Equipment;
        equipment.Init(equipmentBase);
        return equipment;
    }
}
