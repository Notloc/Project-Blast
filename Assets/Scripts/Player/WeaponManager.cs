using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] EquipmentManager equipmentManager = null;

    private bool shootPressed;
    private bool shootHeld;
    private Collider[] colliders;

    private void Awake()
    {
        equipmentManager.OnEquip += RefreshColliders;
        RefreshColliders();
    }

    private void Update()
    {
        TakeInput();
    }

    private void FixedUpdate()
    {
        PerformInput();
    }

    private void TakeInput()
    {
        shootPressed = shootPressed || Input.GetButtonDown("Fire1");
        shootHeld = Input.GetButton("Fire1");
    }

    private void PerformInput()
    {
        GunBehaviour gun = equipmentManager.GetGun();
        if (!gun)
            return;

        if ((gun.IsAutomatic && shootHeld) || shootPressed)
            gun.Fire(colliders);

        shootPressed = false;
    }

    private void RefreshColliders()
    {
        colliders = GetComponentsInChildren<Collider>();
    }
}
