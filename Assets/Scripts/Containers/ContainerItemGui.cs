using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ContainerItemGui : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText = null;
    [SerializeField] TextMeshProUGUI countText = null;
    [SerializeField] Button button = null;
    private ContainerItem item = ContainerItem.None;

    public void SetItem(ContainerItem cItem, UnityAction<Item> onClick)
    {
        this.item = cItem;
        nameText.text = cItem.item.GetName();
        countText.text = cItem.count.ToString();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick(item.item));
    }

    public ContainerItem GetItem()
    {
        return item;
    }

    public void SetSelected()
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }
}
