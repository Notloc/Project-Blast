using ProjectBlast.CameraScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBlast.PlayerScripts
{
    public class Player : Actor
    {
        [SerializeField] ContainerBehaviour containerBehaviour = null;
        [SerializeField] PlayerController playerController = null;

        private void Awake()
        {
            SetActiveController(playerController);
        }

        private void Start()
        {
            CameraManager.Instance.SetCameraController(CameraControllerType.PLAYER, transform);
        }

        public Container GetInventory()
        {
            return containerBehaviour.GetContainer();
        }

        public PlayerController GetPlayerController()
        {
            return playerController;
        }

        public override void Lock()
        {
            base.Lock();
            playerController.SetControlsActive(false);
        }

        public override void Unlock()
        {
            base.Unlock();
            playerController.SetControlsActive(true);
        }

        public override void SetActiveController(IController controller)
        {
            base.SetActiveController(controller);
            if (activeController == null)
            {
                activeController = playerController;
                activeController.SetInput(inputScript);
                activeController.enabled = true;
                activeController.SetControlsActive(!isLocked);
            }
        }
    }
}