using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBlast.Items.Containers
{
    [System.Serializable]
    public class ContainerContents
    {
        public int width { get; private set; }
        public int height { get; private set; }

        [SerializeField] private List<ContainerItemInstance> itemData = new List<ContainerItemInstance>();
        [SerializeField] private ContainerItemGrid containerGrid;

        public ContainerContents(int width, int height)
        {
            this.width = width;
            this.height = height;
            containerGrid = new ContainerItemGrid(width, height);
        }

        public IList<ContainerItemInstance> GetItems()
        {
            return itemData.AsReadOnly();
        }

        public bool AddItem(ItemInstance item, Vector2Int coordinates)
        {
            if (!containerGrid.IsSlotsClear(item.Size, coordinates))
                return false;

            itemData.Add(new ContainerItemInstance(item, coordinates));
            containerGrid.FillSlots(item.Size, coordinates);
            return true;
        }

        public bool RemoveItem(ItemInstance item)
        {
            for (int i = 0; i < this.itemData.Count; i++)
            {
                ContainerItemInstance data = itemData[i];
                if (data.Item.Equals(item))
                {
                    containerGrid.ClearSlots(item.Size, data.Coordinates);
                    itemData.RemoveAt(i);
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