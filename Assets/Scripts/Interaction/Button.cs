using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject target = null;
    public void Interact()
    {
        if (target)
        {
            IActivate activator = target.GetComponent<IActivate>();
            if (activator != null)
                activator.Activate();
        }
    }
}
