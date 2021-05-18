using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectBlast.Items.Containers.Gui
{
    public static class ContainerItemDragManager
    {
        private static Container originContainer;
        private static HashSet<Vector2Int> originCoordinates = new HashSet<Vector2Int>();
        private static List<Vector2Int> coordinateBuffer = new List<Vector2Int>(10);

        private static ContainerGui previousHoverGui;
        private static List<RaycastResult> uiRaycastBuffer = new List<RaycastResult>(10);

        public static void OnItemDragStart(ContainerItemGui itemGui, Container container)
        {
            ContainerItemInstance itemData = itemGui.GetItemData();
            ItemInstance item = itemData.Item;
            originContainer = container;

            originCoordinates.Clear();
            for (int x = 0; x < item.Size.x; x++)
                for (int y = 0; y < item.Size.y; y++)
                    originCoordinates.Add(itemGui.GetCoordinates() + new Vector2Int(x, y));
        }

        public static void OnItemDrag(ContainerItemGui itemGui, Container originContainer)
        {
            ContainerItemInstance itemData = itemGui.GetItemData();
            ItemInstance item = itemData.Item;

            int pixelSize = ContainerSlotGui.SLOT_SIZE_PIXELS;
            RectTransform rect = (RectTransform)itemGui.transform;
            Vector2Int dragCoordinates = Vector2Int.RoundToInt(new Vector2(rect.anchoredPosition.x / pixelSize, -rect.anchoredPosition.y / pixelSize));

            ClearItemHover();

            // Get hovered container
            ContainerGui targetContainerGui = RaycastForContainerGui();
            if (!targetContainerGui)
                return;

            // Determine coordinates item will occupy
            Container targetContainer = targetContainerGui.GetContainer();
            coordinateBuffer.Clear();
            for (int x = 0; x < item.Size.x; x++)
            {
                for (int y = 0; y < item.Size.y; y++)
                {
                    Vector2Int coordinates = dragCoordinates + new Vector2Int(x, y);
                    if (originContainer != targetContainer || !originCoordinates.Contains(coordinates))
                    {
                        coordinateBuffer.Add(coordinates);
                    }
                }
            }

            bool isClear = targetContainer.Contents.IsCoordinatesClear(coordinateBuffer);
            ItemHover(item, dragCoordinates, targetContainerGui, isClear);
        }

        public static void OnItemDragEnd(ContainerItemGui itemGui, Container originContainer)
        {
            ContainerItemInstance itemData = itemGui.GetItemData();
            ItemInstance item = itemData.Item;

            int pixelSize = ContainerSlotGui.SLOT_SIZE_PIXELS;
            RectTransform rect = (RectTransform)itemGui.transform;
            Vector2Int dragCoordinates = Vector2Int.RoundToInt(new Vector2(rect.anchoredPosition.x / pixelSize, -rect.anchoredPosition.y / pixelSize));

            ClearItemHover();

            // Get hovered container
            ContainerGui targetContainerGui = RaycastForContainerGui();
            if (!targetContainerGui)
                return;

            // Determine coordinates item will occupy
            Container targetContainer = targetContainerGui.GetContainer();
            coordinateBuffer.Clear();
            for (int x = 0; x < item.Size.x; x++)
            {
                for (int y = 0; y < item.Size.y; y++)
                {
                    Vector2Int coordinates = dragCoordinates + new Vector2Int(x, y);
                    if (originContainer != targetContainer || !originCoordinates.Contains(coordinates))
                    {
                        coordinateBuffer.Add(coordinates);
                    }
                }
            }

            bool isClear = targetContainer.Contents.IsCoordinatesClear(coordinateBuffer);
            if (isClear)
            {
                originContainer.RemoveItem(item);
                targetContainer.AddItem(item, dragCoordinates);
                itemGui.SetCoordinates(dragCoordinates);

                Vector2Int displayCoords = dragCoordinates;
                displayCoords.y = -displayCoords.y;
                rect.anchoredPosition = displayCoords * pixelSize;
            }
            else
            {
                Vector2Int displayCoords = itemGui.GetCoordinates();
                displayCoords.y = -displayCoords.y;
                rect.anchoredPosition = displayCoords * pixelSize;
            }
        }

        private static ContainerGui RaycastForContainerGui()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = Input.mousePosition;
            EventSystem.current.RaycastAll(eventDataCurrentPosition, uiRaycastBuffer);
            if (uiRaycastBuffer.Count == 0)
                return null;

            return uiRaycastBuffer[0].gameObject.GetComponentInParent<ContainerGui>();
        }

        private static void ItemHover(ItemInstance item, Vector2Int itemCoordinates, ContainerGui targetGui, bool isValid)
        {
            targetGui.HoverItem(item, itemCoordinates, isValid);
            previousHoverGui = targetGui;
        }

        private static void ClearItemHover()
        {
            if (previousHoverGui)
                previousHoverGui.ClearHover();
            previousHoverGui = null;
        }
    }
}