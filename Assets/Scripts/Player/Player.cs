using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] ContainerBehaviour inventoryContainer = null;
    [SerializeField] EquipmentManager equipmentManager = null;

    public Container Inventory { get { return inventoryContainer.GetContainer(); } }
    public EquipmentManager Equipment { get { return equipmentManager; } }
}