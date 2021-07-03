using Notloc.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ItemInstance
{
    public UnityAction<ItemInstance> OnItemUpdated;

    public ItemBase ItemBase => item;
    [SerializeField] ItemBase item;
    
    public Sprite Sprite => item.Sprite;
    public virtual Vector2Int Size => item.BaseSize;

    public virtual string Name => item.name;
    public string Description => item.Description;
    public IList<string> Tags => item.Tags;

    public ItemInstance(ItemBase item)
    {
        this.item = item;
    }
}
