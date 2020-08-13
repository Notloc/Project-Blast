using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] ContainerBehaviour containerBehaviour = null;
    [SerializeField] PlayerController controller = null;

    private void Start()
    {
        CameraManager.Instance.SetCameraController(CameraControllerType.PLAYER, transform);
    }

    public Container GetInventory()
    {
        return containerBehaviour.GetContainer();
    }

    public PlayerController GetController()
    {
        return controller;
    }
}
