using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

public interface IContainer
{
    ContainerType ContainerType { get; }
    ItemDictionary Items { get; }
    bool Add(List<ContainerItem> itemsToAdd);
    bool Remove(List<ContainerItem> itemsToRemove);
}

[System.Serializable]
public class ItemDictionary : SerializableDictionaryBase<ScriptableItem, ContainerItem> {}