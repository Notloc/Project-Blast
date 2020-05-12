using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ContainerItemGui : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Button button = null;
    [SerializeField] TextMeshProUGUI nameText = null;
    [SerializeField] TextMeshProUGUI countText = null;
    private ContainerItem cItem = ContainerItem.None;

    private void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    public void SetItem(ContainerItem cItem)
    {
        this.cItem = cItem;
        nameText.text = cItem.item.GetName();
        countText.text = cItem.count.ToString();
    }

    public ContainerItem GetItem()
    {
        return cItem;
    }

    public void SetSelected()
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    private void OnClick()
    {
        Equipment equipment = cItem.item as Equipment;
        if (equipment)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            Player player = playerObj.GetComponent<Player>();
            if (player)
                player.Equipment.Equip(equipment);
        }
    }
}
