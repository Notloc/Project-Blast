using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugContainerGui : MonoBehaviour
{
    [SerializeField] NewContainer container = null;
    [SerializeField] ContainerView containerView = null;

    private void Start()
    {
        containerView.SetContainer(container);
    }

    [EasyButtons.Button]
    public void UpdateContainer()
    {
        containerView.SetContainer(container);
    }
}
