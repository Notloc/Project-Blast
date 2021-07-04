using Notloc.Utility;
using ProjectBlast.Items.Containers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBlast.Items.Draggable
{
    public class ItemDragGui : MonoBehaviour, IDraggableItem
    {
        [SerializeField] Image itemImage = null;

        public bool IsRotated => isRotated;
        private bool isRotated = false;

        public ContainerEntry ContainerEntry => GetContainerItem();

        public IDraggableItemReceiver ParentDragItemReceiver => dragItem != null ? dragItem.ParentDragItemReceiver : null;

        public RectTransform RectTransform => (RectTransform)transform;

        private IDraggableItem dragItem;

        public void SetDragItem(IDraggableItem dragItem)
        {
            this.dragItem = dragItem;
            if (dragItem == null)
            {
                return;
            }

            itemImage.sprite = dragItem.ContainerEntry.Item.Sprite;
            isRotated = dragItem.ContainerEntry.IsRotated;
            UpdateGraphics();
        }

        public Vector2Int GetSize()
        {
            return isRotated == dragItem.ContainerEntry.IsRotated ? dragItem.ContainerEntry.Size : dragItem.ContainerEntry.Size.Swap();
        }

        public void Rotate()
        {
            isRotated = !isRotated;
            UpdateGraphics();
        }

        private void UpdateGraphics()
        {
            RectTransform rect = (RectTransform)transform;
            rect.sizeDelta = dragItem.ContainerEntry.Item.Size * ContainerSlotGui.SLOT_SIZE_PIXELS;
            if (isRotated)
                rect.localRotation = Quaternion.Euler(0f, 0f, -90f);
            else
                rect.localRotation = Quaternion.identity;
        }

        private ContainerEntry GetContainerItem()
        {
            return new ContainerEntry(dragItem.ContainerEntry, isRotated);
        }
    }
}