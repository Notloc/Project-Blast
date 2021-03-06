﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : ScriptableItem
{
    [SerializeField] ItemBase itemBase;
    [SerializeField] uint itemId;
    public override bool IsUnique { get { return _isUnique; } }
    [SerializeField] private bool _isUnique = false;

    public void Init(ItemBase itemBase)
    {
        this.itemBase = itemBase;
        itemId = itemBase.GetId();
    }

    public override ItemBase GetBase()
    {
        return itemBase;
    }
    public GameObject GetModel()
    {
        return itemBase.GetModel();
    }

    public string GetName()
    {
        return itemBase.GetName();
    }

    public float GetWeight()
    {
        return itemBase.GetWeight();
    }
}
