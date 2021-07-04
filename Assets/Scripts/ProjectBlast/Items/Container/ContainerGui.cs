using ProjectBlast.Items.Draggable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBlast.Items.Containers.Gui
{
    public abstract class ContainerGui : MonoBehaviour, IDraggableItemReceiver, IDraggableItemHoverReceiver
    {
        public abstract RectTransform ItemParent { get; }

        public abstract IContainer GetContainer();
        public abstract void SetContainer(IContainer container);
        public abstract void ClearContainerView();

        public abstract void HoverItem(IDraggableItem dragItem, Vector2 mousePosition);
        public abstract void ClearHover();

        public abstract void ReceiveDraggedItem(IDraggableItem draggedItem, Vector2 mousePosition);
        public abstract void RemoveDraggedItem(IDraggableItem draggedItem);
    }
}