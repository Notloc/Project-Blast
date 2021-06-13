using Notloc.Utility;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBlast.Items.Containers.Gui
{
    public class ContainerGui : MonoBehaviour
    {
        [SerializeField] ContainerSlotGui containerSlotPrefab = null;
        [SerializeField] ContainerItemGui containerItemPrefab = null;
        [SerializeField] GridLayoutGroup grid = null;
        [SerializeField] RectTransform itemArea = null;
        [SerializeField] Color validHoverColor = Color.green;
        [SerializeField] Color invalidHoverColor = Color.red;
         
        private List<ContainerSlotGui> activeSlots = new List<ContainerSlotGui>();

        private ObjectPool<ContainerSlotGui> containerSlotPool = new ObjectPool<ContainerSlotGui>();
        private static readonly int DEFAULT_SLOT_POOL_SIZE = 400;

        private ObjectPool<ContainerItemGui> containerItemPool = new ObjectPool<ContainerItemGui>();
        private static readonly int DEFAULT_ITEM_POOL_SIZE = 250;

        private Container activeContainer;

        public RectTransform ItemParent => itemArea;

        private void Awake()
        {
            containerSlotPool.Initialize(containerSlotPrefab, DEFAULT_SLOT_POOL_SIZE);
            containerItemPool.Initialize(containerItemPrefab, DEFAULT_ITEM_POOL_SIZE);
        }

        public Container GetContainer() => activeContainer;
        public void SetContainer(Container container)
        {
            if (this.activeContainer)
            {
                activeContainer.OnAddItem -= OnAddItem;
                activeContainer.OnRemoveItem -= OnRemoveItem;
            }

            ClearContainerView();
            activeContainer = container;
            if (activeContainer == null)
                return;

            activeContainer.OnAddItem += OnAddItem;
            activeContainer.OnRemoveItem += OnRemoveItem;

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
            IList<ContainerItemInstance> items = activeContainer.GetItems();
            List<ContainerItemGui> itemGuis = containerItemPool.Get(items.Count);
            for (int i = 0; i < itemGuis.Count; i++)
            {
                SetupContainerItemGui(items[i], itemGuis[i]);
            }
        }

        public void ClearContainerView()
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

        public void HoverItem(Vector2Int itemDimensions, Vector2Int coordinates, bool isValid)
        {
            Vector2Int dimensions = activeContainer.Dimensions;
            Color color = isValid ? validHoverColor : invalidHoverColor;

            for (int x = 0; x < itemDimensions.x; x++)
            {
                int x2 = coordinates.x + x;
                if (x2 >= dimensions.x || x2 < 0)
                    continue;

                for (int y = 0; y < itemDimensions.y; y++)
                {
                    int y2 = coordinates.y + y;
                    if (y2 >= dimensions.y || y2 < 0)
                        continue;

                    int index = (y2 * dimensions.x) + x2;
                    activeSlots[index].SetTint(color);
                }
            }
        }

        public void ClearHover()
        {
            foreach (ContainerSlotGui slot in activeSlots)
            {
                slot.ClearTint();
            }
        }

        private void OnAddItem(ContainerItemInstance itemInstance)
        {
            ContainerItemGui itemGui = containerItemPool.Get();
            SetupContainerItemGui(itemInstance, itemGui);
        }

        private void OnRemoveItem(ContainerItemInstance itemInstance)
        {
            if (itemGuiMap.ContainsKey(itemInstance))
            {
                containerItemPool.ReturnToPool(itemGuiMap[itemInstance]);
                itemGuiMap.Remove(itemInstance);
            }
        }

        private Dictionary<ContainerItemInstance, ContainerItemGui> itemGuiMap = new Dictionary<ContainerItemInstance, ContainerItemGui>();

        private void SetupContainerItemGui(ContainerItemInstance itemInstance, ContainerItemGui itemGui)
        {
            itemGui.gameObject.SetActive(true);
            itemGui.transform.SetParent(itemArea, false);

            itemGui.SetItemInstance(itemInstance);
            itemGui.SetContainer(activeContainer);
            itemGui.SetCoordinates(itemInstance.Coordinates);

            itemGuiMap.Add(itemInstance, itemGui);
        }
    }
}