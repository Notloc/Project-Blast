using ProjectBlast.Interaction;
using ProjectBlast.PlayerScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBlast.Debug
{
    public class ContainerSpew : MonoBehaviour, IInteractable
    {
        public void Interact(Player player)
        {
            var container = GetComponent<ContainerBehaviour>();
            container.EmptyIntoWorld();
        }
    }
}