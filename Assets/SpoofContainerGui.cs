using ProjectBlast.Items.Containers;
using ProjectBlast.Items.Containers.Gui;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IContainer))]
public class SpoofContainerGui : ContainerGui
{
    [SerializeField] SpoofContainer container = null;
    [SerializeField] RectTransform itemParent = null;

    public override RectTransform ItemParent => itemParent;

    public override void ClearContainerView() {}

    public override void ClearHover() {}

    public override IContainer GetContainer()
    {
        return container;
    }

    public override void HoverItem(Vector2Int itemDimensions, Vector2Int coordinates, bool isValid) {}

    public override void SetContainer(IContainer container)
    {
        throw new System.NotImplementedException();
    }
}
