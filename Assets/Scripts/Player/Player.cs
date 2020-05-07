using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] ContainerBehaviour containerBehaviour = null;

    public Container GetInventory()
    {
        return containerBehaviour.GetContainer();
    }
}
