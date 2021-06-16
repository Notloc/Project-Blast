using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class GuiWindow : MonoBehaviour
{
    private RectTransform rectT;
    [SerializeField] RectTransform dragArea = null;
    [SerializeField] Button closeButton = null;

    public UnityAction OnClose;

    private void Awake()
    {
        rectT = (RectTransform)transform;
        GuiDrag drag = dragArea.gameObject.AddComponent<GuiDrag>();
        drag.onDrag.AddListener(OnDrag);

        closeButton.onClick.AddListener(Close);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    private void OnDrag(Vector2 delta)
    {
        rectT.anchoredPosition = rectT.anchoredPosition + delta;
    }

    private void OnDisable()
    {
        OnClose?.Invoke();
    }

    public void BringToFront()
    {
        transform.SetAsLastSibling();
    }
}
