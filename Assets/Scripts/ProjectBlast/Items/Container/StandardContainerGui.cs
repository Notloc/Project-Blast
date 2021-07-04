using Notloc.Utility;
using ProjectBlast.Items.Draggable;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBlast.Items.Containers.Gui
{
    public class StandardContainerGui : ContainerGui
    {
        [SerializeField] ContainerSlotGui containerSlotPrefab = null;
        [SerializeField] ContainerEntryGui containerItemPrefab = null;
        [SerializeField] GridLayoutGroup grid = null;
        [SerializeField] RectTransform itemArea = null;
        [SerializeField] Color validHoverColor = Color.green;
        [SerializeField] Color invalidHoverColor = Color.red;
         
        private List<ContainerSlotGui> activeSlots = new List<ContainerSlotGui>();

        private ObjectPool<ContainerSlotGui> containerSlotPool = new ObjectPool<ContainerSlotGui>();
        private static readonly int DEFAULT_SLOT_POOL_SIZE = 200;

        private ObjectPool<ContainerEntryGui> containerItemPool = new ObjectPool<ContainerEntryGui>();
        private static readonly int DEFAULT_ITEM_POOL_SIZE = 100;

        private IContainer activeContainer;

        public override RectTransform ItemParent => itemArea;

        private bool initialized = false;

        public override IContainer GetContainer() => activeContainer;
        public override void SetContainer(IContainer container)
        {
            if (!initialized)
            {
                containerSlotPool.Initialize(containerSlotPrefab, DEFAULT_SLOT_POOL_SIZE);
                containerItemPool.Initialize(containerItemPrefab, DEFAULT_ITEM_POOL_SIZE);
                initialized = true;
            }

            if (this.activeContainer != null)
            {
                this.activeContainer.OnAddItem -= OnAddItem;
                this.activeContainer.OnRemoveItem -= OnRemoveItem;
                this.activeContainer.OnUpdateItem -= OnUpdateItem;
            }

            ClearContainerView();
            activeContainer = container;
            if (activeContainer == null)
                return;

            activeContainer.OnAddItem += OnAddItem;
            activeContainer.OnRemoveItem += OnRemoveItem;
            activeContainer.OnUpdateItem += OnUpdateItem;

            UpdateGrid();
            UpdateItems();
        }

        private void UpdateGrid()
        {
            activeSlots = containerSlotPool.Get(activeContainer.Size);
            for (int i = 0; i < activeSlots.Count; i++)
            {
                var slot = activeSlots[i];
                slot.gameObject.SetActive(true);
                slot.transform.SetParent(grid.transform, false);
                slot.Assign(activeContainer, i);
            }

            ResizeGrid(activeContainer.Width, activeContainer.Height);
        }

        private void ResizeGrid(int x, int y)
        {
            RectTransform rect = (RectTransform)grid.transform;

            Vector2 cellSize = grid.cellSize;
            Vector2 spacing = grid.spacing;

            rect.sizeDelta = new Vector2(
                cellSize.x * x + spacing.x * (x - 1),
                cellSize.y * y + spacing.y * (y - 1)
                );

            itemArea.sizeDelta = rect.sizeDelta;
        }

        private void UpdateItems()
        {
            IList<ContainerEntry> items = activeContainer.GetItems();
            List<ContainerEntryGui> itemGuis = containerItemPool.Get(items.Count);
            for (int i = 0; i < itemGuis.Count; i++)
            {
                SetupItemGui(items[i], itemGuis[i]);
            }
        }

        public override void ClearContainerView()
        {
            if (activeContainer == null)
                return;

            containerItemPool.ReturnToPool(itemGuiMap.Values);
            itemGuiMap.Clear();

            foreach (var slot in activeSlots)
                slot.Unassign();

            containerSlotPool.ReturnToPool(activeSlots);
            activeContainer = null;
        }

        private void OnAddItem(ContainerEntry itemInstance)
        {
            ContainerEntryGui itemGui = containerItemPool.Get();
            SetupItemGui(itemInstance, itemGui);
        }

        private void OnRemoveItem(ContainerEntry containerItem)
        {
            ItemInstance item = containerItem.Item;
            if (itemGuiMap.ContainsKey(item))
            {
                containerItemPool.ReturnToPool(itemGuiMap[item]);
                itemGuiMap.Remove(item);
            }
        }

        private void OnUpdateItem(ContainerEntry containerItem)
        {
            if (!itemGuiMap.ContainsKey(containerItem.Item))
                return;

            ItemInstance item = containerItem.Item;
            if (itemGuiMap.ContainsKey(item))
            {
                ContainerEntryGui itemGui = itemGuiMap[item];
                itemGui.gameObject.SetActive(true);
                itemGui.SetContainerItem(this, containerItem);
            }
        }

        private Dictionary<ItemInstance, ContainerEntryGui> itemGuiMap = new Dictionary<ItemInstance, ContainerEntryGui>();
        private void SetupItemGui(ContainerEntry containerItem, ContainerEntryGui itemGui)
        {
            itemGui.gameObject.SetActive(true);
            itemGui.transform.SetParent(itemArea, false);

            itemGui.SetContainerItem(this, containerItem);
            itemGui.SetContainer(activeContainer);

            itemGuiMap.Add(containerItem.Item, itemGui);
        }

        

        public override void HoverItem(IDraggableItem dragItem, Vector2 mousePosition)
        {
            Vector2Int dragCoordinates = CalculateDragCoordinates(dragItem, mousePosition);


            ContainerEntry itemEntry = dragItem.ContainerEntry;


            Vector2Int dimensions = activeContainer.Dimensions;
            Color color = activeContainer.DoesItemFit(itemEntry.Item, dragCoordinates, itemEntry.IsRotated) ? validHoverColor : invalidHoverColor;

            Vector2Int itemDimensions = itemEntry.Size;

            for (int x = 0; x < itemDimensions.x; x++)
            {
                int xCoord = dragCoordinates.x + x;
                if (xCoord >= dimensions.x || xCoord < 0)
                    continue;

                for (int y = 0; y < itemDimensions.y; y++)
                {
                    int yCoord = dragCoordinates.y + y;
                    if (yCoord >= dimensions.y || yCoord < 0)
                        continue;

                    int index = (yCoord * dimensions.x) + xCoord;
                    activeSlots[index].SetTint(color);
                }
            }
        }

        public override void ClearHover()
        {
            foreach (ContainerSlotGui slot in activeSlots)
            {
                slot.ClearTint();
            }
        }

        private Vector2Int CalculateDragCoordinates(IDraggableItem dragItem, Vector2 mousePos)
        {
            Vector2 itemOffset = CalculateDragItemOffsetForCoordinateCalculations(dragItem);

            Vector2 dragPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(this.ItemParent, mousePos, null, out dragPos);
            dragPos += (this.ItemParent.sizeDelta / 2f); // Localize to bottom left of target container
            dragPos -= itemOffset;

            dragPos.y = (dragPos.y - this.ItemParent.sizeDelta.y); // Flip y
            return Vector2Int.RoundToInt(new Vector2(dragPos.x / ContainerSlotGui.SLOT_SIZE_PIXELS, -dragPos.y / ContainerSlotGui.SLOT_SIZE_PIXELS));
        }

        private Vector2 CalculateDragItemOffsetForCoordinateCalculations(IDraggableItem dragItem)
        {
            Vector2 itemOffset = dragItem.RectTransform.sizeDelta / 2f;
            if (dragItem.ContainerEntry.IsRotated)
            {
                itemOffset = itemOffset.Swap();
            }
            itemOffset.y *= -1f;
            return itemOffset;
        }

        public override void ReceiveDraggedItem(IDraggableItem dragItem, Vector2 mousePosition)
        {
            Vector2Int dragCoordinates = CalculateDragCoordinates(dragItem, mousePosition);
            ContainerEntry itemEntry = dragItem.ContainerEntry;

            if (activeContainer.DoesItemFit(itemEntry.Item, dragCoordinates, itemEntry.IsRotated))
            {
                if ((IDraggableItemReceiver)this == dragItem.ParentDragItemReceiver)
                {
                    activeContainer.MoveItem(itemEntry.Item, dragCoordinates, itemEntry.IsRotated);
                }
                else
                {
                    activeContainer.AddItem(itemEntry.Item, dragCoordinates, itemEntry.IsRotated);
                    dragItem.ParentDragItemReceiver.RemoveDraggedItem(dragItem);
                }
            }
        }

        public override void RemoveDraggedItem(IDraggableItem draggedItem)
        {
            activeContainer.RemoveItem(draggedItem.ContainerEntry.Item);
        }
    }
}