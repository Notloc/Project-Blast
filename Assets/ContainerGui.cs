using ProjectBlast.Items.Containers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBlast.Items.Containers.Gui
{
    public abstract class ContainerGui : MonoBehaviour, IDragItemReceiver, IDragItemHoverReceiver
    {
        public abstract RectTransform ItemParent { get; }

        public abstract IContainer GetContainer();
        public abstract void SetContainer(IContainer container);
        public abstract void ClearContainerView();

        public abstract void HoverItem(IDragItem dragItem, Vector2 mousePosition);
        public abstract void ClearHover();

        public abstract void ReceiveDraggedItem(IDragItem draggedItem, Vector2 mousePosition);
        public abstract void RemoveDraggedItem(IDragItem draggedItem);
    }
}