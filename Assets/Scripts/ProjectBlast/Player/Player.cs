using ProjectBlast.CameraScripts;
using ProjectBlast.Items.Containers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBlast.PlayerScripts
{
    public class Player : Actor
    {
        //[SerializeField] ContainerBehaviour containerBehaviour = null;
        [SerializeField] PlayerController playerController = null;

        private void Start()
        {
            CameraManager.Instance.SetCameraController(CameraControllerType.PLAYER, transform);
        }

        public Container GetInventory()
        {
            return null;
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
    }
}