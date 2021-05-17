using ProjectBlast.Items.Containers;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContainerSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image tintImage = null;
    [SerializeField] Color selectedTint = new Color(1f, 1f, 1f, 0.4f);

    private NewContainer container;
    private int index;

    private bool isSelected;

    public void Assign(NewContainer container, int index)
    {
        this.container = container;
        this.index = index;
        isSelected = false;
        UpdateTint();
    }

    public void Unassign()
    {
        this.container = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isSelected = true;
        UpdateTint();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isSelected = false;
        UpdateTint();
    }

    private void UpdateTint()
    {
        tintImage.color = isSelected ? selectedTint : Color.clear;
    }
}
