using System;
using System.Collections;
using System.Collections.Generic;
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

        public void FillSlots(ICollection<Vector2Int> coordinates)
        {
            foreach (Vector2Int coord in coordinates)
            {
                int index = coord.y * width + coord.x;
                FillSlot(gridSlots, index);
            }
        }

        public void FillSlots(ICollection<int> indexes)
        {
            foreach (int index in indexes)
                FillSlot(gridSlots, index);
        }

        public void FillSlot(int index)
        {
            FillSlot(gridSlots, index);
        }

        private static void FillSlot(ContainerItemGridSlot[] slots, int index)
        {
            slots[index].isFilled = true;
        }

        public void ClearSlots(ICollection<int> indexes)
        {
            foreach (int index in indexes)
                ClearSlot(gridSlots, index);
        }

        public void ClearSlots(ICollection<Vector2Int> coordinates)
        {
            foreach (Vector2Int coord in coordinates)
            {
                int index = coord.y * width + coord.x;
                ClearSlot(gridSlots, index);
            }
        }

        public void ClearSlot(int index)
        {
            ClearSlot(gridSlots, index);
        }

        private static void ClearSlot(ContainerItemGridSlot[] slots, int index)
        {
            slots[index].isFilled = false;
        }

        public bool IsSlotsClear(ICollection<int> indexes)
        {
            foreach (int index in indexes)
            {
                if (!IsSlotClear(gridSlots, index))
                    return false;
            }
            return true;
        }

        public bool IsSlotsClear(ICollection<Vector2Int> coordinates)
        {
            foreach (Vector2Int coord in coordinates)
            {
                int index = coord.y * width + coord.x;
                if (!IsSlotClear(gridSlots, index))
                    return false;
            }
            return true;
        }

        public bool IsSlotClear(int index)
        {
            return IsSlotClear(gridSlots, index);
        }

        public static bool IsSlotClear(ContainerItemGridSlot[] slots, int index)
        {
            return slots[index].isFilled;
        }

    }
}