using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ProjectBlast.Gui
{
    public class GuiDrag : MonoBehaviour, IDragHandler
    {
        public UnityEvent<Vector2> onDrag = new UnityEvent<Vector2>();
        public void OnDrag(PointerEventData eventData)
        {
            onDrag?.Invoke(eventData.delta);
        }
    }
}