using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryGui : MonoBehaviour
{
    [SerializeField] ContainerItemGui itemGuiPrefab = null;
    [SerializeField] Transform itemGuiParent = null;
    private Player player;

    private HashSet<ScriptableItem> displayItems = new HashSet<ScriptableItem>();
    private Dictionary<ScriptableItem, ContainerItemGui> displayMap = new Dictionary<ScriptableItem, ContainerItemGui>();

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    public void OpenGui()
    {
        gameObject.SetActive(true);
        UpdateDisplay();
    }

    public void CloseGui()
    {
        gameObject.SetActive(false);
    }



    private void UpdateDisplay()
    {
        var inventory = player.Inventory;
        var contents = inventory.Items;

        HashSet<ScriptableItem> newDisplayItems = new HashSet<ScriptableItem>();
        foreach(var pair in contents)
        {
            ContainerItem cItem = pair.Value;
            newDisplayItems.Add(cItem.Key);
            if (!displayItems.Contains(cItem.Key))
            {
                newDisplayItems.Add(cItem.Key);
                ContainerItemGui gui = Instantiate(itemGuiPrefab, itemGuiParent);
                displayMap.Add(cItem.Key, gui);
                gui.SetItem(cItem);
            }
            else
                displayMap[cItem.Key].SetItem(cItem);
        }

        displayItems.ExceptWith(newDisplayItems);
        foreach(ScriptableItem item in displayItems)
        {
            ContainerItemGui gui;
            displayMap.TryGetValue(item, out gui);

            if (gui)
            {
                Destroy(gui.gameObject);
                displayMap.Remove(item);
            }
        }
        displayItems = newDisplayItems;
    }

    private void Update()
    {
        TakeInput();
    }

    private ContainerItem GetSelected()
    {
        GameObject selected = EventSystem.current.currentSelectedGameObject;
        if (!selected)
            return ContainerItem.None;

        var itemGui = selected.GetComponent<ContainerItemGui>();
        if (!itemGui)
            return ContainerItem.None;

        return itemGui.GetItem();
    }

    private void TakeInput()
    {
        bool modify = false;
        if (Input.GetButton("Inventory Modifier Key"))
            modify = true;

        if (Input.GetButtonDown("Drop Item"))
            DropSelected(modify);
    }

    private void DropSelected(bool modify)
    {
        var selected = GetSelected();
        if (selected.Equals(ContainerItem.None))
            return;

        if (modify && selected.count > 1)
            selected.count = 1;

        if (player.Inventory.Remove(selected))
        {
            UpdateDisplay();
            for(int i=0; i<selected.count; i++)
                Game.Instance.Factories.ItemEntityFactory.CreateItemEntity(selected.item, player.transform.position + player.transform.forward + player.transform.up);
        }
    }
}
