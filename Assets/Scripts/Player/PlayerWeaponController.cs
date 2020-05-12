using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] Player player = null;

    bool pressedPrimary = false;
    bool pressedSecondary = false;

    private void Update()
    {
        TakeInput();
    }

    private void FixedUpdate()
    {
        ProcessInput();
    }

    private void TakeInput()
    {
        pressedPrimary = Input.GetButton("Primary Fire");
        pressedSecondary = Input.GetButton("Secondary Fire");
    }

    private void ProcessInput()
    {

        if (pressedPrimary)
            FireGun();


    }

    private void FireGun()
    {
        var weaponEquipment = player.Equipment.PrimaryWeapon;

        var weapon = weaponEquipment.GetWeapon();
        if (weapon)
            weapon.Fire();
    }

}
