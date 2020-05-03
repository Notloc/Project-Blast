using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerSpew : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        var container = GetComponent<ContainerBehaviour>();
        container.EmptyIntoWorld();
    }
}
