using Notloc.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBlast.Items.Containers.Gui
{
    public class ContainerDragItemGui : MonoBehaviour, IDragItem
    {
        [SerializeField] Image itemImage = null;

        public bool IsRotated => isRotated;
        private bool isRotated = false;

        public ContainerItemEntry ContainerItem => GetContainerItem();

        public IDragItemReceiver ParentDragItemReceiver => dragItem != null ? dragItem.ParentDragItemReceiver : null;

        public RectTransform RectTransform => (RectTransform)transform;

        private IDragItem dragItem;
        
        public void SetDragItem(IDragItem dragItem)
        {
            this.dragItem = dragItem;
            if (dragItem == null)
            {
                return;
            }
            
            itemImage.sprite = dragItem.ContainerItem.Item.Sprite;
            isRotated = dragItem.ContainerItem.IsRotated;
            RotateGraphics();
        }

        public Vector2Int GetSize()
        {
            return isRotated == dragItem.ContainerItem.IsRotated ? dragItem.ContainerItem.Size : dragItem.ContainerItem.Size.Swap();
        }

        public void Rotate()
        {
            isRotated = !isRotated;
            RotateGraphics();
        }

        private void RotateGraphics()
        {
            RectTransform rect = (RectTransform)transform;
            rect.sizeDelta = dragItem.ContainerItem.Size * ContainerSlotGui.SLOT_SIZE_PIXELS;
            if (isRotated)
                rect.localRotation = Quaternion.Euler(0f, 0f, -90f);
            else
                rect.localRotation = Quaternion.identity;
        }

        private ContainerItemEntry GetContainerItem()
        {
            return new ContainerItemEntry(dragItem.ContainerItem, isRotated);
        }
    }
}