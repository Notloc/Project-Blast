using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GuiWindow : MonoBehaviour
{
    private RectTransform rectT;
    [SerializeField] RectTransform dragArea = null;


    private void Awake()
    {
        rectT = (RectTransform)transform;
        GuiDrag drag = dragArea.gameObject.AddComponent<GuiDrag>();
        drag.onDrag.AddListener(OnDrag);
    }

    public void OnDrag(Vector2 delta)
    {
        rectT.anchoredPosition = rectT.anchoredPosition + delta;
    }
}
