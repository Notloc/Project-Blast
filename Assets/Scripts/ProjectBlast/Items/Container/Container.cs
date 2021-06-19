using Notloc.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectBlast.Items.Containers
{
    [CreateAssetMenu]
    public class Container : ScriptableObject, IContainer
    {
        [SerializeField] int width = 2;
        [SerializeField] int height = 2;
        [SerializeField] private List<ContainerItemInstance> itemData = new List<ContainerItemInstance>();
        [SerializeField] private ContainerItemGrid containerGrid;

        public UnityAction<ContainerItemInstance> OnAddItem { get; set; }
        public UnityAction<ContainerItemInstance> OnRemoveItem { get; set; }

        public int Width => width;
        public int Height => height;
        public int Size => Width * Height;
        public Vector2Int Dimensions => new Vector2Int(width, height);

        public void Initialize(int width, int height)
        {
            this.width = width;
            this.height = height;
            containerGrid = new ContainerItemGrid(width, height);
        }

        public IList<ContainerItemInstance> GetItems()
        {
            return itemData.AsReadOnly();
        }

        public bool AddItem(ItemInstance itemInstance)
        {
            if (AddItem(itemInstance, false))
                return true;
            else
                return AddItem(itemInstance, true);
        }

        private bool AddItem(ItemInstance itemInstance, bool isRotated)
        {
            Vector2Int size = isRotated ? itemInstance.BaseSize.Swap() : itemInstance.BaseSize;
            for (int y = 0; y < height && y + size.y <= height; y++) {
                for (int x = 0; x < width && x + size.x <= width; x++)
                {
                    Vector2Int containerPos = new Vector2Int(x, y);
                    if (AddItem(itemInstance, containerPos, isRotated))
                        return true;
                }
            }
            return false;
        }

        public bool AddItem(ItemInstance itemInstance, Vector2Int coordinates, bool isRotated)
        {
            ContainerItemInstance containerItem = new ContainerItemInstance(itemInstance, coordinates, isRotated);
            if (!containerGrid.IsSlotsClear(containerItem.Size, coordinates))
                return false;

            itemData.Add(containerItem);
            containerGrid.FillSlots(containerItem.Size, coordinates);
            OnAddItem?.Invoke(containerItem);
            return true;
        }

        public bool RemoveItem(ItemInstance item)
        {
            for (int i = 0; i < this.itemData.Count; i++)
            {
                ContainerItemInstance itemInstance = itemData[i];
                if (itemInstance.Item.Equals(item))
                {
                    containerGrid.ClearSlots(itemInstance.Size, itemInstance.Coordinates);
                    itemData.RemoveAt(i);
                    OnRemoveItem?.Invoke(itemInstance);
                    return true;
                }
            }
            return false;
        }

        public bool IsCoordinatesClear(Vector2Int coordinates)
        {
            return containerGrid.IsSlotClear(coordinates);
        }

        public bool IsCoordinatesClear(List<Vector2Int> coordinates)
        {
            foreach (Vector2Int coord in coordinates)
            {
                if (!containerGrid.IsSlotClear(coord))
                    return false;
            }
            return true;
        }
    }
}