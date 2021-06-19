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
        [SerializeField] ItemInspectorGuiFactory itemInspectorFactory = null;

        private UnityEvent OnItemDrag = new UnityEvent();
        private UnityEvent<ContainerItemGui, IContainer> OnItemDragStart = new UnityEvent<ContainerItemGui, IContainer>();
        private UnityEvent<ContainerItemGui, IContainer> OnItemDragEnd = new UnityEvent<ContainerItemGui, IContainer>();

        protected RectTransform rect;

        private IContainer container;
        private ContainerItemInstance containerItemInstance;
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

        public ContainerItemInstance GetItemInstance() => containerItemInstance; 
        public void SetItemInstance(ContainerItemInstance itemInstance)
        {
            this.containerItemInstance = itemInstance;
            itemImage.sprite = itemInstance != null ? itemInstance.Item.Sprite : null;
            Resize();
        }

        public IContainer GetContainer() => container;
        public void SetContainer(IContainer container)
        {
            this.container = container;
        }

        protected virtual void Resize()
        {
            rect.sizeDelta = containerItemInstance.Size * ContainerSlotGui.SLOT_SIZE_PIXELS;
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

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.clickCount == 2)
                OpenItemExamine();
        }

        private void OpenItemExamine()
        {
            itemInspectorFactory.CreateItemInspectorGui(containerItemInstance.Item);
        }
    }
}