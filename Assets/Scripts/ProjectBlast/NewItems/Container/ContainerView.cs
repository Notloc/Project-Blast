using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class ContainerView : MonoBehaviour
{
    [SerializeField] NewItem item;


    [SerializeField] ContainerSlot containerSlotPrefab = null;
    [SerializeField] NewContainerItem containerItemPrefab = null;
    [SerializeField] GridLayoutGroup grid = null;
    [SerializeField] RectTransform itemArea = null;

    private List<ContainerSlot> activeSlots = new List<ContainerSlot>();

    private ObjectPool<ContainerSlot> containerSlotPool = new ObjectPool<ContainerSlot>();
    private static readonly int DEFAULT_SLOT_POOL_SIZE = 400;

    private ObjectPool<NewContainerItem> containerItemPool = new ObjectPool<NewContainerItem>();
    private static readonly int DEFAULT_ITEM_POOL_SIZE = 250;

    private NewContainer activeContainer;

    private void Awake()
    {
        containerSlotPool.Initialize(containerSlotPrefab, DEFAULT_SLOT_POOL_SIZE);
        containerItemPool.Initialize(containerItemPrefab, DEFAULT_ITEM_POOL_SIZE);
    }

    public void SetContainer(NewContainer container)
    {
        ClearContainerView();
        activeContainer = container;
        if (!activeContainer)
            return;

        UpdateGrid();
        UpdateItems();
    }

    private void UpdateGrid()
    {
        activeSlots = containerSlotPool.Get(activeContainer.size);
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
        // get items from container
        List<NewItem> items = new List<NewItem>();
        items.Add(item);

        List<NewContainerItem> containerItems = containerItemPool.Get(items.Count);
        for (int i = 0; i < containerItems.Count; i++)
        {
            NewContainerItem containerItem = containerItems[i];
            containerItem.gameObject.SetActive(true);
            containerItem.AssignItem(items[i]);
            containerItem.transform.SetParent(itemArea, false);
            // position in grid
            containerItem.OnItemDrag.AddListener(OnItemDrag);
        }
    }

    public void ClearContainerView()
    {
        if (!activeContainer)
            return;

        foreach (var slot in activeSlots)
            slot.Unassign();

        containerSlotPool.ReturnToPool(activeSlots);
        activeContainer = null;
    }

    private void OnItemDrag(NewContainerItem item)
    {
        
    }
}
