using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// The data representation for all instances of a specific item.
/// Provides the base stats and data.
/// </summary>
[CreateAssetMenu(menuName = "Items/New Item")]
public class ItemBase : ScriptableItem
{
    [SerializeField] private float itemValue = 1f;
    [SerializeField] private float itemWeight = 1f;
    [SerializeField] private GameObject model = null;

    public override bool IsUnique { get { return false; } }
    public override ItemBase GetBase() { return this; }

    private uint _id = 0;
    public uint GetId() { return _id; }

    public string GetName() { return this.name; }
    public float GetValue() { return itemValue; }
    public float GetWeight() { return itemWeight; }
    public GameObject GetModel() { return model; }

    [Obsolete("Do not set an Item's id. Only the database should do this.")]
    public void _SetId(ItemDatabase db, uint id) { this._id = id; }
}
