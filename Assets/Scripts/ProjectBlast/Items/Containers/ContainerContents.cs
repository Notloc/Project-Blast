using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Container Contents")]
public class ContainerContents : ScriptableObject
{
    [SerializeField] List<ItemBaseCount> contents = new List<ItemBaseCount>();

    public ICollection<ItemBaseCount> GetContents()
    {
        return contents.AsReadOnly();
    }
}

[System.Serializable]
public struct ItemBaseCount
{
    public ItemBase itemBase;
    public uint count;
}
