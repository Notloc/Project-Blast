using ProjectBlast.Gui;
using ProjectBlast.Items.Containers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Animations;
using UnityEngine;

[RequireComponent(typeof(GuiWindow))]
public class ItemInspectGui : MonoBehaviour
{
    [SerializeField] GenericItemInspectGui genericItemInspect = null;
    [SerializeField] ModdableItemInspectGui moddableItemInspect = null;

    private GameObject activeInspect;
    private ItemInstance item;

    private static Dictionary<ItemInstance, ItemInspectGui> openItemInspects = new Dictionary<ItemInstance, ItemInspectGui>();

    public GuiWindow Window { get; private set; }

    private void Awake()
    {
        genericItemInspect.gameObject.SetActive(false);
        moddableItemInspect.gameObject.SetActive(false);

        Window = GetComponent<GuiWindow>();
        Window.OnClose += OnClose;
    }

    private void OnDestroy()
    {
        Window.OnClose -= OnClose;
    }

    public void Open(ItemInstance item, IContainer container)
    {
        //TODO: Fix this stupidity, check it before we make the object instead of destroying it here
        if (openItemInspects.ContainsKey(item))
        {
            openItemInspects[item].Window.BringToFront();
            Destroy(gameObject);
        }
        else
        {
            this.item = item;

            if (item as ModdableItemInstance != null)
            {
                activeInspect = moddableItemInspect.gameObject;
                moddableItemInspect.gameObject.SetActive(true);
                moddableItemInspect.SetItem(item, container);
            }
            else
            {
                activeInspect = genericItemInspect.gameObject;
                genericItemInspect.gameObject.SetActive(true);
                genericItemInspect.SetItem(item, container);
            }
            this.item.OnItemRelocated += HandleRelocation;
            openItemInspects.Add(item, this);
        }
    }

    private void HandleRelocation(ItemInstance item)
    {
        Window.Close();
    }

    private void OnClose()
    {
        if (activeInspect)
            activeInspect.gameObject.SetActive(false);
        activeInspect = null;

        if (item != null)
        {
            openItemInspects.Remove(item);
            item.OnItemRelocated -= HandleRelocation;
        }
        item = null;
    }
}
