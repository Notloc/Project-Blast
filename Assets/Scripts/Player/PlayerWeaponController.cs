using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] Player player = null;
    [SerializeField] float zeroingDistance = 250f;

    [SerializeField] Collider[] playerColliders = null;

    new Camera camera;

    bool pressedPrimary = false;
    bool pressedSecondary = false;


    private void Start()
    {
        camera = Camera.main;
    }

    private void LateUpdate()
    {
        TakeInput();
        PointGun();
        ProcessInput();
    }

    private void TakeInput()
    {
        pressedPrimary = Input.GetButton("Primary Fire");
        pressedSecondary = Input.GetButton("Secondary Fire");
    }

    private void PointGun()
    {
        var equipment = player.Equipment.PrimaryWeapon;
        if (!equipment)
            return;

        var weapon = equipment.GetWeapon();
        if (weapon)
            weapon.Aim(camera.transform.position, camera.transform.forward, zeroingDistance);
    }

    private void ProcessInput()
    {

        if (pressedPrimary)
            FireGun();
    }

    private void FireGun()
    {
        var equipment = player.Equipment.PrimaryWeapon;
        if (!equipment)
            return;

        var weapon = equipment.GetWeapon();
        if (weapon)
            weapon.Fire(playerColliders);
    }

}
