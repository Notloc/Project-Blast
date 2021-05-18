using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ProjectBlast.Items.Containers.Gui
{
    public class ContainerItemGui : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] Image itemImage = null;
        [SerializeField] Vector2Int size = Vector2Int.one;

        private UnityEvent<ContainerItemGui, Container> OnItemDrag = new UnityEvent<ContainerItemGui, Container>();
        private UnityEvent<ContainerItemGui, Container> OnItemDragStart = new UnityEvent<ContainerItemGui, Container>();
        private UnityEvent<ContainerItemGui, Container> OnItemDragEnd = new UnityEvent<ContainerItemGui, Container>();

        private RectTransform rect;

        private Container container;
        private ContainerItemInstance itemData;
        private Vector2Int coordinates;

        private void Awake()
        {
            rect = (RectTransform)transform;
            rect.sizeDelta = size * ContainerSlotGui.SLOT_SIZE_PIXELS;

            OnItemDragStart.AddListener(ContainerItemDragManager.OnItemDragStart);
            OnItemDrag.AddListener(ContainerItemDragManager.OnItemDrag);
            OnItemDragEnd.AddListener(ContainerItemDragManager.OnItemDragEnd);
        }

        public Vector2Int GetCoordinates() => coordinates;
        public void SetCoordinates(Vector2Int coordinates)
        {
            this.coordinates = coordinates;
            Vector2Int anchorPos = coordinates;
            anchorPos.y = -anchorPos.y;
            rect.anchoredPosition = anchorPos * ContainerSlotGui.SLOT_SIZE_PIXELS;
        }

        public ContainerItemInstance GetItemData() => itemData; 
        public void SetItemData(ContainerItemInstance itemData)
        {
            this.itemData = itemData;
            itemImage.sprite = itemData != null ? itemData.Item.Sprite : null;
            Resize();
        }

        public Container GetContainer() => container;
        public void SetContainer(Container container)
        {
            this.container = container;
        }

        private void Resize()
        {
            rect.sizeDelta = itemData.Item.Size * ContainerSlotGui.SLOT_SIZE_PIXELS;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            itemImage.raycastTarget = false;
            OnItemDragStart?.Invoke(this, container);
        }

        public void OnDrag(PointerEventData eventData)
        {
            rect.anchoredPosition += eventData.delta;


            Container targetContainer = container; // TODO, get this via raycast or something
            OnItemDrag?.Invoke(this, targetContainer);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            itemImage.raycastTarget = true;
            Container targetContainer = container; // TODO, get this via raycast or something
            OnItemDragEnd?.Invoke(this, targetContainer);
        }
    }
}