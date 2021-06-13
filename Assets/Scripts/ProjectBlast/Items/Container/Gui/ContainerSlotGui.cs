using ProjectBlast.Items.Containers;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContainerSlotGui : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static readonly int SLOT_SIZE_PIXELS = 64;

    [SerializeField] Image tintImage = null;
    [SerializeField] Color selectedTint = new Color(1f, 1f, 1f, 0.4f);

    private Container container;
    private int index;

    private bool isSelected;
    private bool isTinted;

    private Color customTint = Color.clear;

    public void Assign(Container container, int index)
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
        tintImage.color = isTinted ? customTint : isSelected ? selectedTint : Color.clear;
    }

    public void SetTint(Color customTint)
    {
        isTinted = true;
        this.customTint = customTint;
        UpdateTint();
    }

    public void ClearTint()
    {
        isTinted = false;
        UpdateTint();
    }
}
