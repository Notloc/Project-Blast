using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum CameraControllerType
{
    PLAYER,
    SPACESHIP,
}

public class CameraManager : MonoBehaviour
{
    [SerializeField] PlayerCameraController playerCameraController = null;
    [SerializeField] SpaceshipCameraController spaceshipCameraController = null;
    public static CameraManager Instance { get; private set; }
    private CameraController activeCameraController;

    private void Awake()
    {
        Instance = this;
    }

    public void SetCameraController(CameraControllerType type, Transform target)
    {
        switch(type)
        {
            case CameraControllerType.PLAYER:
                SetActiveCameraController(playerCameraController, target);
                break;
            case CameraControllerType.SPACESHIP:
                SetActiveCameraController(spaceshipCameraController, target);
                break;
        }
    }

    private void SetActiveCameraController(CameraController newController, Transform target)
    {
        if (activeCameraController)
            activeCameraController.Disable();

        activeCameraController = newController;
        activeCameraController.Enable(target);
    }
}

