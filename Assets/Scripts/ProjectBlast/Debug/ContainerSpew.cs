using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerSpew : MonoBehaviour, IInteractable
{
    public void Interact(Player player)
    {
        var container = GetComponent<ContainerBehaviour>();
        container.EmptyIntoWorld();
    }
}
