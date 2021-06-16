using Notloc.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemInstance
{
    public ItemBase ItemBase => item;
    [SerializeField] ItemBase item;
    
    public Sprite Sprite => item.Sprite;
    public virtual Vector2Int BaseSize => item.BaseSize;

    public virtual string Name => item.name;
    public string Description => item.Description;
    public IList<string> Tags => item.Tags;

    public ItemInstance(ItemBase item)
    {
        this.item = item;
    }
}
