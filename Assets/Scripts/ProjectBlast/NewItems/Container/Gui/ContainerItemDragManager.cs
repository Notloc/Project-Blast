using Notloc.Utility;
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
        
        public void OnItemDragStart(ContainerItemGui itemGui, Container container)
        {
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



        public void OnItemDrag(ContainerItemGui itemGui, Container originContainer)
        {
            ContainerItemInstance itemData = dragItemGui.DragItemInstance;
            Vector2 mousePos = Mouse.current.position.ReadValue();

            // Move drag item
            RectTransform dragItemRect = dragItemGui.transform.RectTransform();
            Vector2 offset = dragItemRect.sizeDelta/2f;
            dragItemRect.anchoredPosition = mousePos - offset;


            ClearItemHover();
            // Get hovered container
            ContainerGui targetContainerGui = RaycastForContainerGui();
            if (!targetContainerGui)
                return;

            Vector2 dragPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(targetContainerGui.ItemParent, Mouse.current.position.ReadValue() - offset, null, out dragPos);
            
            int pixelSize = ContainerSlotGui.SLOT_SIZE_PIXELS;
            Vector2Int dragCoordinates = Vector2Int.RoundToInt(new Vector2(dragPos.x / pixelSize, -dragPos.y / pixelSize));

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
            ContainerItemInstance itemData = dragItemGui.DragItemInstance;
            HideDragItem();

            // Get hovered container
            ContainerGui targetContainerGui = RaycastForContainerGui();
            if (!targetContainerGui)
                return;

            RectTransform dragItemRect = dragItemGui.transform.RectTransform();
            Vector2 offset = dragItemRect.sizeDelta / 2f;

            Vector2 dragPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(targetContainerGui.ItemParent, Mouse.current.position.ReadValue() - offset, null, out dragPos);

            int pixelSize = ContainerSlotGui.SLOT_SIZE_PIXELS;
            Vector2Int dragCoordinates = Vector2Int.RoundToInt(new Vector2(dragPos.x / pixelSize, -dragPos.y / pixelSize));

            ClearItemHover();

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
        }
    }
}