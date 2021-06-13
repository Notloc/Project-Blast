using Notloc.Utility;
using ProjectBlast.Engine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace ProjectBlast.Items.Containers.Gui
{
    [CreateAssetMenu]
    public class ContainerItemDragManager : ScriptableObject
    {
        [SerializeField] ContainerDragItemGui dragItemGuiPrefab = null;

        private HashSet<Vector2Int> originCoordinates = new HashSet<Vector2Int>();
        private List<Vector2Int> coordinateBuffer = new List<Vector2Int>(10);

        private ContainerGui previousHoverGui;
        private List<RaycastResult> uiRaycastBuffer = new List<RaycastResult>(10);

        private ContainerDragItemGui dragItemGui;
        private Container originContainer;

        public void OnItemDragStart(ContainerItemGui itemGui, Container container)
        {
            this.originContainer = container;
            UpdateOriginContainerSlots(itemGui);
            SetDragItem(itemGui);

            InputManager.mainInput.Inventory.RotateItem.performed += OnRotateItem;
        }

        /// <summary>
        /// Calculates which slots the item being dragged originally occupied.
        /// </summary>
        /// <param name="itemGui"></param>
        private void UpdateOriginContainerSlots(ContainerItemGui itemGui)
        {
            ContainerItemInstance item = itemGui.GetItemInstance();
            originCoordinates.Clear();
            for (int x = 0; x < item.Size.x; x++)
                for (int y = 0; y < item.Size.y; y++)
                    originCoordinates.Add(itemGui.GetCoordinates() + new Vector2Int(x, y));
        }

        private void SetDragItem(ContainerItemGui itemGui)
        {
            if (!dragItemGui)
                CreateDragItem();

            dragItemGui.gameObject.SetActive(true);
            dragItemGui.SetContainerItemGui(itemGui);
        }

        private void HideDragItem()
        {
            dragItemGui.gameObject.SetActive(false);
        }

        private void CreateDragItem()
        {
            dragItemGui = Instantiate(dragItemGuiPrefab, GameObject.FindGameObjectWithTag("Main Canvas").transform);
        }



        public void OnItemDrag()
        {
            ContainerItemInstance itemData = dragItemGui.DragItemInstance;
            Vector2 mousePos = Mouse.current.position.ReadValue();

            PositionDragItem(mousePos);

            ClearItemHover();

            // Get hovered container
            ContainerGui targetContainerGui = RaycastForContainerGui();
            if (!targetContainerGui)
                return;

            Vector2Int dragCoordinates = CalculateDragCoordinates(mousePos, targetContainerGui);

            // Determine coordinates item will occupy
            Container targetContainer = targetContainerGui.GetContainer();
            coordinateBuffer.Clear();
            for (int x = 0; x < itemData.Size.x; x++)
            {
                for (int y = 0; y < itemData.Size.y; y++)
                {
                    Vector2Int coordinates = dragCoordinates + new Vector2Int(x, y);
                    if (originContainer != targetContainer || !originCoordinates.Contains(coordinates))
                    {
                        coordinateBuffer.Add(coordinates);
                    }
                }
            }

            // Show preview
            bool isClear = targetContainer.IsCoordinatesClear(coordinateBuffer);
            ItemHover(itemData, dragCoordinates, targetContainerGui, isClear);
        }

        public void OnItemDragEnd(ContainerItemGui itemGui, Container originContainer)
        {
            HideDragItem();
            ClearItemHover();

            // Get hovered container
            ContainerGui targetContainerGui = RaycastForContainerGui();
            if (!targetContainerGui)
                return;

            Vector2Int dragCoordinates = CalculateDragCoordinates(Mouse.current.position.ReadValue(), targetContainerGui);

            ContainerItemInstance itemData = dragItemGui.DragItemInstance;

            // Determine coordinates item will occupy
            Container targetContainer = targetContainerGui.GetContainer();
            coordinateBuffer.Clear();
            for (int x = 0; x < itemData.Size.x; x++)
            {
                for (int y = 0; y < itemData.Size.y; y++)
                {
                    Vector2Int coordinates = dragCoordinates + new Vector2Int(x, y);
                    if (originContainer != targetContainer || !originCoordinates.Contains(coordinates))
                    {
                        coordinateBuffer.Add(coordinates);
                    }
                }
            }

            bool isClear = targetContainer.IsCoordinatesClear(coordinateBuffer);
            if (isClear)
            {
                originContainer.RemoveItem(itemData.Item);
                targetContainer.AddItem(itemData.Item, dragCoordinates, itemData.IsRotated);
            }

            InputManager.mainInput.Inventory.RotateItem.performed -= OnRotateItem;
        }

        private ContainerGui RaycastForContainerGui()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, uiRaycastBuffer);
            if (uiRaycastBuffer.Count == 0)
                return null;

            return uiRaycastBuffer[0].gameObject.GetComponentInParent<ContainerGui>();
        }

        private void ItemHover(ContainerItemInstance item, Vector2Int itemCoordinates, ContainerGui targetGui, bool isValid)
        {
            targetGui.HoverItem(item.Size, itemCoordinates, isValid);
            previousHoverGui = targetGui;
        }

        private void ClearItemHover()
        {
            if (previousHoverGui)
                previousHoverGui.ClearHover();
            previousHoverGui = null;
        }

        private void OnRotateItem(CallbackContext context)
        {
            dragItemGui.Rotate();
            OnItemDrag();
        }

        private void PositionDragItem(Vector2 mousePos)
        {
            RectTransform dragItemRect = dragItemGui.transform.RectTransform();
            dragItemRect.anchoredPosition = mousePos - CalculateItemOffset();
        }

        private Vector2Int CalculateDragCoordinates(Vector2 mousePos, ContainerGui targetContainerGui)
        {
            Vector2 itemOffset = CalculateItemOffset();

            Vector2 dragPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(targetContainerGui.ItemParent, Mouse.current.position.ReadValue(), null, out dragPos);
            dragPos += (targetContainerGui.ItemParent.sizeDelta / 2f); // Localize to bottom left of target container
            
            dragPos.x = dragPos.x - itemOffset.x;

            if (dragItemGui.DragItemInstance.IsRotated)
                dragPos.y = dragPos.y - itemOffset.y;
            else
                dragPos.y = dragPos.y + itemOffset.y;

            dragPos.y = (dragPos.y - targetContainerGui.ItemParent.sizeDelta.y); // Flip y
            return Vector2Int.RoundToInt(new Vector2(dragPos.x / ContainerSlotGui.SLOT_SIZE_PIXELS, -dragPos.y / ContainerSlotGui.SLOT_SIZE_PIXELS));
        }

        private Vector2 CalculateItemOffset()
        {
            RectTransform dragItemRect = dragItemGui.transform.RectTransform();
            if (dragItemGui.DragItemInstance.IsRotated)
            {
                Vector2 itemOffset = dragItemRect.sizeDelta.Swap() / 2f;
                itemOffset.y = itemOffset.y - dragItemRect.sizeDelta.x;
                return itemOffset;
            }
            else
            {
                return dragItemRect.sizeDelta / 2f;
            }
        }
    }
}