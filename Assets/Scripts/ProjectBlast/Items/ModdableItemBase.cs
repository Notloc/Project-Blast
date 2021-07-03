using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModdableItemBase : ItemBase
{

    [Header("Mods")]
    [SerializeField] List<ItemModSlotData> modSlots;
    public IList<ItemModSlotData> ModSlots => modSlots;

}
