using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ContainerGui : MonoBehaviour
{
    public abstract RectTransform ItemParent { get; }

    public abstract IContainer GetContainer();
    public abstract void SetContainer(IContainer container);

    public abstract void ClearContainerView();

    public abstract void HoverItem(Vector2Int itemDimensions, Vector2Int coordinates, bool isValid);
    public abstract void ClearHover();

}
