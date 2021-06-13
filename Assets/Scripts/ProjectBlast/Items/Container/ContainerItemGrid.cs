using Notloc.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEditor.UIElements;
using UnityEngine;

namespace ProjectBlast.Items.Containers
{
    [Serializable]
    public class ContainerItemGrid
    {
        [SerializeField] private ContainerItemGridSlot[] gridSlots;
        public int width { get; private set; }
        public int height { get; private set; }

        public ContainerItemGrid(int width, int height)
        {
            this.width = width;
            this.height = height;
            gridSlots = new ContainerItemGridSlot[width * height];
        }

        public void FillSlots(Vector2Int size, Vector2Int coordinates)
        {
            for (int x = 0; x < size.x; x++) {
                for (int y = 0; y < size.y; y++)
                {
                    Vector2Int coord = coordinates + new Vector2Int(x, y);
                    int index = coord.y * width + coord.x;
                    gridSlots[index].isFilled = true;
                }
            }
        }

        public void ClearSlots(Vector2Int size, Vector2Int coordinates)
        {
            for (int x = 0; x < size.x; x++) {
                for (int y = 0; y < size.y; y++)
                {
                    Vector2Int coord = coordinates + new Vector2Int(x, y);
                    int index = coord.y * width + coord.x;
                    gridSlots[index].isFilled = false;
                }
            }
        }

        public bool IsSlotsClear(Vector2Int size, Vector2Int coordinates)
        {
            for (int x = 0; x < size.x; x++)
                for (int y = 0; y < size.y; y++) 
                    if (!IsSlotClear(coordinates + new Vector2Int(x, y)))
                        return false;
            
            return true;
        }

        public bool IsValidCoordinates(Vector2Int coordinates)
        {
            if (coordinates.x < 0 || coordinates.y < 0 || coordinates.x >= width || coordinates.y >= height)
                return false;
            return true;
        }

        public bool IsSlotsClear(ICollection<Vector2Int> coordinates)
        {
            foreach (Vector2Int coord in coordinates)
            {
                if (!IsValidCoordinates(coord))
                    return false;

                int index = coord.y * width + coord.x;
                if (!IsSlotClear(gridSlots, index))
                    return false;
            }
            return true;
        }

        public bool IsSlotClear(Vector2Int coordinates)
        {
            if (!IsValidCoordinates(coordinates))
                return false;
            int index = coordinates.y * width + coordinates.x;
            return IsSlotClear(gridSlots, index);
        }

        private static bool IsSlotClear(ContainerItemGridSlot[] slots, int index)
        {
            if (index < 0 || index >= slots.Length)
                return false; // Out of Bounds
            return !slots[index].isFilled;
        }

    }
}