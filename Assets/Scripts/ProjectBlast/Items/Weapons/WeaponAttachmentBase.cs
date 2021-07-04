using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponAttachmentBase : ModdableItemBase
{
    public Vector2Int SizeModifier => sizeModifier;
    [SerializeField] Vector2Int sizeModifier = Vector2Int.zero;
}

