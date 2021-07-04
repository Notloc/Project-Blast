using ProjectBlast.Items.Draggable;
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
    public class ContainerEntryGui : MonoBehaviour, IDraggableItem, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
    {
        [SerializeField] Image itemImage = null;
        [SerializeField] Vector2Int size = Vector2Int.one;
        [SerializeField] ItemDragManager itemDragManager = null;
        [SerializeField] ItemInspectorGuiFactory itemInspectorFactory = null;

        private UnityEvent<IDraggableItem> OnItemDragStart = new UnityEvent<IDraggableItem>();
        private UnityEvent OnItemDrag = new UnityEvent();
        private UnityEvent<IDraggableItem> OnItemDragEnd = new UnityEvent<IDraggableItem>();

        public RectTransform RectTransform => (RectTransform)transform;

        private IContainer container;
        private ContainerEntry containerEntry;
        private IDraggableItemReceiver parent;

        private void Awake()
        {
            ((RectTransform)transform).sizeDelta = size * ContainerSlotGui.SLOT_SIZE_PIXELS;

            OnItemDragStart.AddListener(itemDragManager.OnItemDragStart);
            OnItemDrag.AddListener(itemDragManager.OnItemDrag);
            OnItemDragEnd.AddListener(itemDragManager.OnItemDragEnd);
        }

        

        public ContainerEntry ContainerEntry => containerEntry;

        public IDraggableItemReceiver ParentDragItemReceiver => parent;

        public void SetContainerItem(IDraggableItemReceiver parent, ContainerEntry containerItem)
        {
            this.parent = parent;
            this.containerEntry = containerItem;
            SetPosition(containerItem.Coordinates);
            itemImage.sprite = containerItem.Item != null ? containerItem.Item.Sprite : null;
            Resize();
        }
        private void SetPosition(Vector2Int coordinates)
        {
            Vector2Int anchorPos = coordinates;
            anchorPos.y = -anchorPos.y;
            ((RectTransform)transform).anchoredPosition = anchorPos * ContainerSlotGui.SLOT_SIZE_PIXELS;
        }


        public IContainer GetContainer() => container;
        public void SetContainer(IContainer container)
        {
            this.container = container;
        }

        protected virtual void Resize()
        {
            ((RectTransform)transform).sizeDelta = containerEntry.Size * ContainerSlotGui.SLOT_SIZE_PIXELS;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            SetRaycastTarget(false);
            itemImage.enabled = false;
            OnItemDragStart?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnItemDrag?.Invoke();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            SetRaycastTarget(true);
            itemImage.enabled = true;
            OnItemDragEnd?.Invoke(this);
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
            itemInspectorFactory.CreateItemInspectorGui(containerEntry.Item, container);
        }
    }
}