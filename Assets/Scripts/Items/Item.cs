using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : ScriptableItem
{
    [SerializeField] ItemBase itemBase;
    [SerializeField] uint itemId;
    public override bool IsUnique { get { return _isUnique; } }
    [SerializeField] private bool _isUnique = false;

    public virtual void Init(ItemBase itemBase)
    {
        this.itemBase = itemBase;
        itemId = itemBase.GetId();
    }

    public override ItemBase GetItemBase()
    {
        return itemBase;
    }
    public GameObject GetModel()
    {
        return itemBase.GetModel();
    }
}
