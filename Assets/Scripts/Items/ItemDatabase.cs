using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

/// <summary>
/// Manages all the ItemBases in the game
/// This will automatically handle assigning ids to ItemBases
/// Letting us to not worry about it... or mess it up
/// It even tracks what Items gets deleted
/// </summary>
[CreateAssetMenu(menuName = "Databases/Create Item Database")]
public class ItemDatabase : ScriptableObject
{
    // Automatically handles retrieving the instance when first loaded
    public static ItemDatabase Instance {
        get { return _instance ? _instance : FindInstance(); }
    }
    private static ItemDatabase _instance;
    private static ItemDatabase FindInstance()
    {
        _instance = Resources.FindObjectsOfTypeAll<ItemDatabase>().FirstOrDefault();
        return _instance;
    }

    // The ItemBase to use if an id is requested but not found in the database
    [SerializeField] private ItemBase defaultItem = null;
    //New items to register this update
    [SerializeField] private List<ItemBase> addItems = new List<ItemBase>();

    // The registered items that have been given an id
    [SerializeField] [HideInInspector]
    private ItemMap registeredItems = new ItemMap();

    // Mapping of ids to names
    [SerializeField] [HideInInspector]
    private StringMap nameMap = new StringMap();

    // Ids that have gone missing and need to be mapped to a new id
    private List<uint> missingIds = new List<uint>();

    // Mapping of missing ids to what their names were
    [SerializeField] [HideInInspector]
    private StringMap missingNameMap = new StringMap();


    // The ever increasing id counter
    [SerializeField] [HideInInspector]
    private uint idCounter = 1;


    public ItemMap GetItemMap()
    {
        ItemMap newMap = new ItemMap();
        registeredItems.CopyTo(newMap);
        return newMap;
    }

    public StringMap GetMissingMap()
    {
        StringMap newMap = new StringMap();
        missingNameMap.CopyTo(newMap);
        return newMap;
    }



    // Suppress the deprecation warning for _SetId, since the DB is intended to use it
#pragma warning disable 612, 618
    public void UpdateDatabase()
    {
        if (!defaultItem)
        {
            Debug.LogError("Cannot update the database. A default item MUST be assigned.");
            return;
        }

        // Add the default item
        registeredItems.Remove(0);
        defaultItem._SetId(this, 0);
        registeredItems.Add(0, defaultItem);

        // Add new items
        var existingValues = registeredItems.Values;
        foreach(ItemBase newItem in addItems)
        {
            // No double registering
            if (existingValues.Contains(newItem))
                continue;

            newItem._SetId(this, idCounter);
            registeredItems.Add(idCounter, newItem);
            idCounter++;
        }
        addItems.Clear();

        //Remap missing items that have been filled out
        //

        // Remove missing items/ids
        List<uint> missingValues = new List<uint>(10);
        foreach (var pair in registeredItems)
        {
            if (!pair.Value)
                missingValues.Add(pair.Key);
        }
        foreach (uint key in missingValues)
            registeredItems.Remove(key);
        
        // Add the missing items' ids for remapping
        foreach(uint id in missingValues)
        {
            string name = "Unknown";
            nameMap.TryGetValue(id, out name);

            missingIds.Add(id);
            missingNameMap.Add(id, name);
        }

        // Regenerate name map
        nameMap.Clear();
        foreach (var pair in registeredItems)
        {
            nameMap.Add(pair.Key, pair.Value.GetName());
        }
    }
#pragma warning restore 612, 618
}

[System.Serializable]
public class ItemMap : SerializableDictionaryBase<uint, ItemBase> {}

[System.Serializable]
public class StringMap : SerializableDictionaryBase<uint, string> {}

[System.Serializable]
public class IdMap : SerializableDictionaryBase<uint, uint> {}
