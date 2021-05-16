using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NewItem : ScriptableObject
{
    [SerializeField] Sprite sprite = null;
    [SerializeField] Vector2Int size = Vector2Int.one;

    public Sprite Sprite => sprite;
    public Vector2Int Size => size;
}
