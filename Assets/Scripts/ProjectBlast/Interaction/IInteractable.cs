﻿using ProjectBlast.PlayerScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBlast.Interaction
{
    public interface IInteractable
    {
        void Interact(Player player);
    }
}