using ProjectBlast.Items.Containers.Gui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugWeaponItemGui : MonoBehaviour
{
    [SerializeField] WeaponBehaviour weaponBehaviour = null;
    [SerializeField] ContainerGui gui = null;

    private void Start()
    {
        gui.GetContainer().AddItem(weaponBehaviour.WeaponInstance);
    }
}
