using Notloc.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ItemInstance
{
    public UnityAction<ItemInstance> OnItemUpdated;
    public UnityAction<ItemInstance> OnItemRelocated;

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

    public ItemInstance Parent => parent;
    [SerializeField] ItemInstance parent;

    public void SetParent(ItemInstance parent)
    {
        if (this == parent)
        {
            throw new System.Exception("An item cannot be its own parent!");
        }
        this.parent = parent;
    }

    public ItemInstance GetTopLevelParent()
    {
        if (parent != null)
        {
            return parent.GetTopLevelParent();
        }
        return this;
    }

    public void TriggerRelocationEvent()
    {
        OnItemRelocated?.Invoke(this);
    }
}
