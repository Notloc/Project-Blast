﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiManager : MonoBehaviour
{
    [SerializeField] InventoryGui inventory = null;
    [SerializeField] GameObject crosshair = null;
    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        inventory.CloseGui();
        crosshair.SetActive(true);
    }

    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            if (!inventory.isActiveAndEnabled)
                inventory.OpenGui();
            else
                inventory.CloseGui();

            crosshair.SetActive(!inventory.isActiveAndEnabled);
            player.GetController().SetControlsActive(!inventory.isActiveAndEnabled);
        }
    }
}
