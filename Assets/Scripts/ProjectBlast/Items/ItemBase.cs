using Notloc.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemBase : UpdatableData
{
    public Vector2Int BaseSize => size;
    [Header("Item Data")]
    [SerializeField] Vector2Int size = Vector2Int.one;

    public Sprite Sprite => sprite;
    [SerializeField] Sprite sprite = null;

    public GameObject ModelPrefab => modelPrefab.gameObject;
    [SerializeField] ItemModel modelPrefab = null;
}
