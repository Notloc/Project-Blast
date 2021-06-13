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

    public ItemInstance(ItemBase item)
    {
        this.item = item;
    }
}
