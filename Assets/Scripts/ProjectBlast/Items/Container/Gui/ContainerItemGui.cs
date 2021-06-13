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
    public class ContainerItemGui : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
    {
        [SerializeField] Image itemImage = null;
        [SerializeField] Vector2Int size = Vector2Int.one;
        [SerializeField] ContainerItemDragManager itemDragManager = null;

        private UnityEvent OnItemDrag = new UnityEvent();
        private UnityEvent<ContainerItemGui, Container> OnItemDragStart = new UnityEvent<ContainerItemGui, Container>();
        private UnityEvent<ContainerItemGui, Container> OnItemDragEnd = new UnityEvent<ContainerItemGui, Container>();

        private RectTransform rect;

        private Container container;
        private ContainerItemInstance itemInstance;
        private Vector2Int coordinates;

        private void Awake()
        {
            rect = (RectTransform)transform;
            rect.sizeDelta = size * ContainerSlotGui.SLOT_SIZE_PIXELS;

            OnItemDragStart.AddListener(itemDragManager.OnItemDragStart);
            OnItemDrag.AddListener(itemDragManager.OnItemDrag);
            OnItemDragEnd.AddListener(itemDragManager.OnItemDragEnd);
        }

        public Vector2Int GetCoordinates() => coordinates;
        public void SetCoordinates(Vector2Int coordinates)
        {
            this.coordinates = coordinates;
            Vector2Int anchorPos = coordinates;
            anchorPos.y = -anchorPos.y;
            rect.anchoredPosition = anchorPos * ContainerSlotGui.SLOT_SIZE_PIXELS;
        }

        public ContainerItemInstance GetItemInstance() => itemInstance; 
        public void SetItemInstance(ContainerItemInstance itemInstance)
        {
            this.itemInstance = itemInstance;
            itemImage.sprite = itemInstance != null ? itemInstance.Item.Sprite : null;
            Resize();
        }

        public Container GetContainer() => container;
        public void SetContainer(Container container)
        {
            this.container = container;
        }

        private void Resize()
        {
            rect.sizeDelta = itemInstance.Size * ContainerSlotGui.SLOT_SIZE_PIXELS;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            SetRaycastTarget(false);
            itemImage.enabled = false;
            OnItemDragStart?.Invoke(this, container);
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnItemDrag?.Invoke();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            SetRaycastTarget(true);
            itemImage.enabled = true;
            OnItemDragEnd?.Invoke(this, container);
        }

        public void SetRaycastTarget(bool state)
        {
            itemImage.raycastTarget = state;
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            Debug.Log(eventData.clickCount);
        }
    }
}