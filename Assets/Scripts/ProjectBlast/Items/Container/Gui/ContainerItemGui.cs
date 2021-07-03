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
    public class ContainerItemGui : MonoBehaviour, IDragItem, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
    {
        [SerializeField] Image itemImage = null;
        [SerializeField] Vector2Int size = Vector2Int.one;
        [SerializeField] ContainerItemDragManager itemDragManager = null;
        [SerializeField] ItemInspectorGuiFactory itemInspectorFactory = null;

        private UnityEvent<IDragItem> OnItemDragStart = new UnityEvent<IDragItem>();
        private UnityEvent OnItemDrag = new UnityEvent();
        private UnityEvent<IDragItem> OnItemDragEnd = new UnityEvent<IDragItem>();

        public RectTransform RectTransform => rect;
        protected RectTransform rect;

        private IContainer container;
        private ContainerItemEntry containerItemEntry;
        private IDragItemReceiver parent;

        private void Awake()
        {
            rect = (RectTransform)transform;
            rect.sizeDelta = size * ContainerSlotGui.SLOT_SIZE_PIXELS;

            OnItemDragStart.AddListener(itemDragManager.OnItemDragStart);
            OnItemDrag.AddListener(itemDragManager.OnItemDrag);
            OnItemDragEnd.AddListener(itemDragManager.OnItemDragEnd);
        }

        

        public ContainerItemEntry ContainerItem => containerItemEntry;

        public IDragItemReceiver ParentDragItemReceiver => parent;

        public void SetContainerItem(IDragItemReceiver parent, ContainerItemEntry containerItem)
        {
            this.parent = parent;
            this.containerItemEntry = containerItem;
            SetPosition(containerItem.Coordinates);
            itemImage.sprite = containerItem.Item != null ? containerItem.Item.Sprite : null;
            Resize();
        }
        private void SetPosition(Vector2Int coordinates)
        {
            Vector2Int anchorPos = coordinates;
            anchorPos.y = -anchorPos.y;
            rect.anchoredPosition = anchorPos * ContainerSlotGui.SLOT_SIZE_PIXELS;
        }


        public IContainer GetContainer() => container;
        public void SetContainer(IContainer container)
        {
            this.container = container;
        }

        protected virtual void Resize()
        {
            rect.sizeDelta = containerItemEntry.Size * ContainerSlotGui.SLOT_SIZE_PIXELS;
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
            itemInspectorFactory.CreateItemInspectorGui(containerItemEntry.Item, container);
        }
    }
}