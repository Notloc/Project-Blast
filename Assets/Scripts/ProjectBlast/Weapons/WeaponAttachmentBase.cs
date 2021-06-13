using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponAttachmentBase : ModdableItemBase
{
    public string SlotName => slotName;
    [SerializeField] string slotName = "SLOT_NAME";

    public Vector2Int SizeModifier => sizeModifier;
    [SerializeField] Vector2Int sizeModifier = Vector2Int.zero;
}
