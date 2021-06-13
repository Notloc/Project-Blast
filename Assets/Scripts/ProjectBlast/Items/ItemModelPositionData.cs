using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemModelPositionData : MonoBehaviour
{
    public IList<ItemModSlotPositionData> ModSlotPositions => modSlotPositionData.AsReadOnly();
    [SerializeField] List<ItemModSlotPositionData> modSlotPositionData = new List<ItemModSlotPositionData>();

    public Dictionary<string, ItemModSlotPositionData> ModSlotPositionsByName { get; private set; }

    private void Awake()
    {
        ModSlotPositionsByName = Util.Dictionary(modSlotPositionData, modSlot => modSlot.SlotName);
    }
}
