using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBlast.Items.Containers.Gui
{
    public class ContainerDragItemGui : MonoBehaviour
    {
        [SerializeField] Image itemImage = null;
        private ContainerItemInstance dragItemInstance;
        public ContainerItemInstance DragItemInstance => dragItemInstance;

        public void SetContainerItemGui(ContainerItemGui itemGui)
        {
            ContainerItemInstance target = itemGui.GetItemInstance();
            dragItemInstance = new ContainerItemInstance(target.Item, target.Coordinates, target.IsRotated);
            Resize();
        }

        public void Rotate()
        {
            dragItemInstance.Rotate();
            Resize();
        }

        private void Resize()
        {
            RectTransform rect = (RectTransform)transform;
            rect.sizeDelta = dragItemInstance.Size * ContainerSlotGui.SLOT_SIZE_PIXELS;
        }
    }
}