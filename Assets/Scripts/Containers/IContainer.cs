using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

public interface IContainer
{
    ItemDictionary Items { get; }
    bool Add(List<ItemCount> itemsToAdd);
    bool Remove(List<ItemCount> itemsToRemove);
}

[System.Serializable]
public class ItemDictionary : SerializableDictionaryBase<ScriptableItem, ItemCount> {}