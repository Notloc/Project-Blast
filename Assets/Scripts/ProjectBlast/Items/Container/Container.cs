using Notloc.Utility;
using ProjectBlast.Items.Containers.Gui;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectBlast.Items.Containers
{
    [CreateAssetMenu]
    public class Container : ScriptableObject, IContainer
    {
        [SerializeField] int width = 2;
        [SerializeField] int height = 2;
        [SerializeField] private List<ContainerEntry> containerItems = new List<ContainerEntry>();
        [SerializeField] private Dictionary<ItemInstance, ContainerEntry> containerItemCache = new Dictionary<ItemInstance, ContainerEntry>();

        [SerializeField] private ContainerItemGrid containerGrid;

        public UnityAction<ContainerEntry> OnAddItem { get; set; }
        public UnityAction<ContainerEntry> OnRemoveItem { get; set; }
        public UnityAction<ContainerEntry> OnUpdateItem { get; set; }

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

        public IList<ContainerEntry> GetItems()
        {
            return containerItems.AsReadOnly();
        }

        /// <summary>
        /// Try to fit the item into the container.
        /// </summary>
        /// <param name="itemInstance"></param>
        /// <returns></returns>
        public bool AddItem(ItemInstance itemInstance)
        {
            if (AddItem(itemInstance, false))
                return true;
            else
                return AddItem(itemInstance, true);
        }

        private bool AddItem(ItemInstance itemInstance, bool isRotated)
        {
            Vector2Int size = isRotated ? itemInstance.Size.Swap() : itemInstance.Size;
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

        /// <summary>
        /// Add the item to a specific spot in the container
        /// </summary>
        /// <param name="itemInstance"></param>
        /// <param name="coordinates"></param>
        /// <param name="isRotated"></param>
        /// <returns></returns>
        public bool AddItem(ItemInstance itemInstance, Vector2Int coordinates, bool isRotated)
        {
            ContainerEntry containerItem = new ContainerEntry(itemInstance, this, coordinates, isRotated);
            if (!containerGrid.IsSlotsClear(containerItem.Size, coordinates))
                return false;

            containerItems.Add(containerItem);
            containerItemCache.Add(itemInstance, containerItem);
            containerGrid.FillSlots(containerItem);

            itemInstance.OnItemUpdated += OnItemUpdated;

            OnAddItem?.Invoke(containerItem);
            itemInstance.TriggerRelocationEvent();
            return true;
        }

        public bool MoveItem(ItemInstance itemInstance, Vector2Int coordinates, bool isRotated)
        {
            ContainerEntry existingItem = containerItemCache[itemInstance];
            HashSet<Vector2Int> existingItemCoordinates = GetCoordinateSet(existingItem);

            ContainerEntry newItem = new ContainerEntry(itemInstance, this, coordinates, isRotated);
            if (!containerGrid.IsSlotsClear(newItem.Size, coordinates, existingItemCoordinates))
                return false;

            containerItems.Remove(existingItem);
            containerItemCache.Remove(itemInstance);
            containerGrid.ClearSlots(existingItem);

            containerItems.Add(newItem);
            containerItemCache.Add(itemInstance, newItem);
            containerGrid.FillSlots(newItem);

            OnUpdateItem?.Invoke(newItem);
            return true;
        }

        public bool RemoveItem(ItemInstance item)
        {
            if (!containerItemCache.ContainsKey(item))
            {
                return false;
            }

            ContainerEntry containerItem = containerItemCache[item];

            containerItems.Remove(containerItem);
            containerItemCache.Remove(item);
            containerGrid.ClearSlots(containerItem);

            OnRemoveItem?.Invoke(containerItem);

            item.OnItemUpdated -= OnItemUpdated;
            item.TriggerRelocationEvent();
            return true;
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

        private void OnItemUpdated(ItemInstance itemInstance)
        {
            ContainerEntry containerItem = containerItemCache[itemInstance];
            if (containerItem.Item != null)
            {
                if (containerItem.Size != itemInstance.Size)
                {
                    ResizeItem(containerItem);
                }
                else
                {
                    OnUpdateItem?.Invoke(containerItem);
                }
            }
        }

        public bool WillResizedItemFit(ItemInstance item, ItemInstance other, Vector2Int sizeMod)
        {
            if (sizeMod.x <= 0 && sizeMod.y <= 0)
                return true;

            Vector2Int newSize = item.Size + sizeMod;
            ContainerEntry containerItem = containerItemCache[item];
            if (containerItem.IsRotated)
            {
                newSize = newSize.Swap();
                sizeMod = sizeMod.Swap();
            }

            HashSet<Vector2Int> originalCoordinates;
            if (containerItemCache.ContainsKey(other))
            {
                originalCoordinates = GetCoordinateSet(containerItem, containerItemCache[other]);
            }
            else
            {
                originalCoordinates = GetCoordinateSet(containerItem);
            }
            
            
            // For each possible direction the item can expand in
            for (int x = 0; x >= -sizeMod.x; x--)
            {
                for (int y = 0; y >= -sizeMod.y; y--)
                {
                    // Check if it fits
                    bool ItemFits()
                    {
                        for (int x2 = 0; x2 < newSize.x; x2++)
                        {
                            for (int y2 = 0; y2 < newSize.y; y2++)
                            {
                                Vector2Int coord = containerItem.Coordinates + new Vector2Int(x + x2, y + y2);
                                if (!originalCoordinates.Contains(coord) && !IsCoordinatesClear(coord))
                                    return false;
                            }
                        }
                        return true;
                    }
                    if (ItemFits())
                        return true;
                }
            }
            return false;
        }

        private void ResizeItem(ContainerEntry containerItem)
        {
            Vector2Int sizeMod = containerItem.Item.Size - (containerItem.IsRotated ? containerItem.Size.Swap() : containerItem.Size);
            Vector2Int newSize = containerItem.Item.Size;

            if (containerItem.IsRotated)
            {
                newSize = newSize.Swap();
                sizeMod = sizeMod.Swap();
            }

            HashSet<Vector2Int> originalCoordinates = GetCoordinateSet(containerItem);

            // Check if it fits
            Vector2Int GetNewItemPosition()
            {
                if (sizeMod.x <= 0 && sizeMod.y <= 0)
                {
                    return containerItem.Coordinates;
                }

                // For each possible direction the item can expand in
                for (int x = 0; x >= -sizeMod.x; x--)
                {
                    for (int y = 0; y >= -sizeMod.y; y--)
                    {
                        bool IsClear()
                        {
                            for (int x2 = 0; x2 < newSize.x; x2++)
                            {
                                for (int y2 = 0; y2 < newSize.y; y2++)
                                {
                                    Vector2Int coord = containerItem.Coordinates + new Vector2Int(x + x2, y + y2);
                                    if (!originalCoordinates.Contains(coord) && !IsCoordinatesClear(coord))
                                        return false;
                                }
                            }
                            return true;
                        }
                        if (IsClear())
                            return containerItem.Coordinates + new Vector2Int(x, y);
                    }
                }
                return -Vector2Int.one;
            }

            Vector2Int newPosition = GetNewItemPosition();
            if (newPosition == -Vector2Int.one)
            {
                newPosition = containerItem.Coordinates;
            }

            MoveItem(containerItem.Item, newPosition, containerItem.IsRotated);
        }

        private HashSet<Vector2Int> GetCoordinateSet(params ContainerEntry[] containerEntries)
        {
            HashSet<Vector2Int> coordinateSet = new HashSet<Vector2Int>();
            foreach (ContainerEntry entry in containerEntries)
            {
                Vector2Int baseCoordinate = entry.Coordinates;
                Vector2Int size = entry.Size;
                for (int x = 0; x < size.x; x++)
                {
                    for (int y = 0; y < size.y; y++)
                    {
                        coordinateSet.Add(new Vector2Int(x + baseCoordinate.x, y + baseCoordinate.y));
                    }
                }
            }
            return coordinateSet;
        }

        public bool DoesItemFit(ItemInstance item, Vector2Int coordinates, bool isRotated)
        {
            HashSet<Vector2Int> coordinateSet = new HashSet<Vector2Int>();
            if (containerItemCache.ContainsKey(item))
            {
                coordinateSet = GetCoordinateSet(containerItemCache[item]);
            }

            Vector2Int size = isRotated ? item.Size.Swap() : item.Size;

            for (int x = 0; x < size.x; x++) {
                for (int y = 0; y < size.y; y++)
                {
                    Vector2Int coord = coordinates + new Vector2Int(x, y);
                    if (!coordinateSet.Contains(coord) && !IsCoordinatesClear(coord))
                        return false;
                }
            }
            return true;
        }
    }
}