using Notloc.Utility;
using ProjectBlast.Engine;
using ProjectBlast.Items.Containers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static UnityEngine.InputSystem.InputAction;

namespace ProjectBlast.Items.Draggable
{
    [CreateAssetMenu]
    public class ItemDragManager : ScriptableObject
    {
        [SerializeField] ItemDragGui dragItemGuiPrefab = null;

        private HashSet<Vector2Int> originCoordinates = new HashSet<Vector2Int>();
        private List<Vector2Int> coordinateBuffer = new List<Vector2Int>(10);

        private IDraggableItemHoverReceiver previousHoverGui;
        private List<RaycastResult> uiRaycastBuffer = new List<RaycastResult>(10);

        private ItemDragGui dragItemGui;

        private void CreateDragItem()
        {
            MainGui mainGui = GameObject.FindGameObjectWithTag(Tags.MAIN_GUI).GetComponent<MainGui>();
            dragItemGui = Instantiate(dragItemGuiPrefab, mainGui.TopParent);
        }

        private void SetDragItem(IDraggableItem dragItem)
        {
            if (!dragItemGui)
                CreateDragItem();

            dragItemGui.gameObject.SetActive(true);
            dragItemGui.SetDragItem(dragItem);
        }

        private void ClearDragItem()
        {
            dragItemGui.SetDragItem(null);
            dragItemGui.gameObject.SetActive(false);
        }

        private void ItemHover(IDraggableItem dragItem, IDraggableItemHoverReceiver hoverReceiver, Vector2 mousePosition)
        {
            hoverReceiver.HoverItem(dragItem, mousePosition);
            previousHoverGui = hoverReceiver;
        }

        private void ClearItemHover()
        {
            if (previousHoverGui != null)
                previousHoverGui.ClearHover();
            previousHoverGui = null;
        }

        private void OnDragRotateItem(CallbackContext context)
        {
            dragItemGui.Rotate();
            OnItemDrag();
        }

        private void PositionDragItem(Vector2 mousePos)
        {
            RectTransform dragItemRect = dragItemGui.transform.RectTransform();
            dragItemRect.anchoredPosition = mousePos - CalculateDragItemMouseOffset();
        }

        private IDraggableItemReceiver RaycastForDragReceiver()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, uiRaycastBuffer);
            if (uiRaycastBuffer.Count == 0)
                return null;

            return uiRaycastBuffer[0].gameObject.GetComponentInParent<IDraggableItemReceiver>();
        }

        private IDraggableItemHoverReceiver RaycastForHoverReceiver()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, uiRaycastBuffer);
            if (uiRaycastBuffer.Count == 0)
                return null;

            return uiRaycastBuffer[0].gameObject.GetComponentInParent<IDraggableItemHoverReceiver>();
        }

        public void OnItemDragStart(IDraggableItem dragItem)
        {
            SetDragItem(dragItem);
            OnItemDrag();
            InputManager.mainInput.Inventory.RotateItem.performed += OnDragRotateItem;
        }

        public void OnItemDrag()
        {
            Vector2Int dragItemSize = dragItemGui.GetSize();
            Vector2 mousePos = Mouse.current.position.ReadValue();

            PositionDragItem(mousePos);
            ClearItemHover();

            // Get hovered container
            IDraggableItemHoverReceiver hoverTarget = RaycastForHoverReceiver();
            if (hoverTarget == null)
                return;

            ItemHover(dragItemGui, hoverTarget, mousePos);
        }

        public void OnItemDragEnd(IDraggableItem dragItem)
        {
            IDraggableItemReceiver dragReceiver = RaycastForDragReceiver();
            if (dragReceiver != null)
            {
                dragReceiver.ReceiveDraggedItem(dragItemGui, Mouse.current.position.ReadValue());
            }

            InputManager.mainInput.Inventory.RotateItem.performed -= OnDragRotateItem;
            ClearDragItem();
            ClearItemHover();
        }

        private Vector2 CalculateDragItemMouseOffset()
        {
            RectTransform dragItemRect = dragItemGui.transform.RectTransform();
            if (dragItemGui.IsRotated)
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

        /// <summary>
        /// Caches which slots the item being dragged originally occupied.
        /// </summary>
        /// <param name="dragItem"></param>
        private void UpdateOriginContainerSlots(IDraggableItem dragItem)
        {
            ContainerEntry item = dragItem.ContainerEntry;
            originCoordinates.Clear();
            for (int x = 0; x < item.Size.x; x++)
                for (int y = 0; y < item.Size.y; y++)
                    originCoordinates.Add(item.Coordinates + new Vector2Int(x, y));
        }
    }
}