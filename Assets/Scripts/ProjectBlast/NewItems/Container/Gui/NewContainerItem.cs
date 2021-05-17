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
    public class NewContainerItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] Image itemImage = null;
        [SerializeField] Vector2Int size = Vector2Int.one;
        [SerializeField] int slotSize = 64;

        private NewItem item;
        public UnityEvent<NewContainerItem> OnItemDrag = new UnityEvent<NewContainerItem>();
        public UnityEvent<NewContainerItem> OnItemDragEnd = new UnityEvent<NewContainerItem>();

        private RectTransform rect;
        private void Awake()
        {
            rect = (RectTransform)transform;
            rect.sizeDelta = size * slotSize;
        }

        public void AssignItem(NewItem item)
        {
            this.item = item;
            Resize();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            itemImage.raycastTarget = false;
            OnItemDrag?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            rect.anchoredPosition += eventData.delta;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            itemImage.raycastTarget = true;
            OnItemDragEnd?.Invoke(this);
        }

        private void Resize()
        {
            rect.sizeDelta = item.Size * slotSize;
        }
    }
}