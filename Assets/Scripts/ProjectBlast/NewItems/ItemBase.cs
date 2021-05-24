using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemBase : ScriptableObject
{
    [SerializeField] Vector2Int size = Vector2Int.one;
    [SerializeField] Sprite sprite = null;

    public Vector2Int BaseSize => size;
    public Sprite Sprite => sprite;
}
