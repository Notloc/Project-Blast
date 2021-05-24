using Notloc.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemInstance
{
    public ItemBase Item => item;
    public Sprite Sprite => item.Sprite;

    public Vector2Int BaseSize => item.BaseSize;

    private ItemBase item;

    public ItemInstance(ItemBase item)
    {
        this.item = item;
    }
}
