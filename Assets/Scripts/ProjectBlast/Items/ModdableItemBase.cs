using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModdableItemBase : ItemBase
{

    [Header("Mods")]
    [SerializeField] List<ItemModSlot> modSlots;
    public IList<ItemModSlot> ModSlots => modSlots;

}
