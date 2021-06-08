using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace ProjectBlast.CameraScripts
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance { get; private set; }
        private CameraController activeCameraController;

        private List<CameraController> registeredCameraControllers = new List<CameraController>();

        private void Awake()
        {
            Instance = this;
        }

        public void SetActiveCameraController(CameraController cameraController)
        {
            registeredCameraControllers.Remove(cameraController); // In case its in the list already

            if (activeCameraController)
            {
                activeCameraController.Disable();
            }

            activeCameraController = cameraController;
            activeCameraController.Enable();
            registeredCameraControllers.Add(cameraController);
        }

        public void DisableCameraController(CameraController cameraController)
        {
            cameraController.Disable();
            registeredCameraControllers.Remove(cameraController);
            if (activeCameraController == cameraController)
            {
                activeCameraController = registeredCameraControllers.LastOrDefault();
                if (activeCameraController)
                {
                    activeCameraController.Enable();
                }
            }
        }
    }
}