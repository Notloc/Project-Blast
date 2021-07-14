using Notloc.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class WeaponBehaviour : MonoBehaviour
{
    public WeaponInstance WeaponInstance { get; private set; }
    [SerializeField] WeaponInstanceHolder weaponInstanceHolder = null;

    private WeaponGraphics weaponGraphics;

    private void Awake()
    {
        weaponGraphics = new WeaponGraphics();
        if (weaponInstanceHolder != null)
        {
            SetWeaponInstance(weaponInstanceHolder.CreateWeaponInstance());
        }
    }

    public void SetWeaponInstance(WeaponInstance weaponInstance)
    {
        if (this.WeaponInstance != null)
        {
            this.WeaponInstance.OnModAdded -= OnAddAttachment;
            this.WeaponInstance.OnModRemoved -= OnRemoveAttachment;
        }

        this.WeaponInstance = weaponInstance;
        this.WeaponInstance.OnModAdded += OnAddAttachment;
        this.WeaponInstance.OnModRemoved += OnRemoveAttachment;

        weaponGraphics.CreateModel(weaponInstance, transform);
    }

    public void OnAddAttachment(ItemModSlotInstance modSlot)
    {
        weaponGraphics.OnAddAttachment(modSlot);
    }

    public void OnRemoveAttachment(ItemModSlotInstance modSlot, ItemInstance removedItem)
    {
        weaponGraphics.OnRemoveAttachment(modSlot, removedItem); 
    }
}
