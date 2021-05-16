using ProjectBlast.PlayerScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBlast.Interaction
{
    public class Button : MonoBehaviour, IInteractable
    {
        [SerializeField] GameObject target = null;
        public void Interact(Player player)
        {
            if (target)
            {
                IActivate activator = target.GetComponent<IActivate>();
                if (activator != null)
                    activator.Activate();
            }
        }
    }
}