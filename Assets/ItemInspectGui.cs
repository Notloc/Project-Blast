using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[RequireComponent(typeof(GuiWindow))]
public class ItemInspectGui : MonoBehaviour
{
    [SerializeField] GenericItemInspectGui genericItemInspect = null;
    [SerializeField] WeaponItemInspectGui weaponItemInspect = null;

    private GameObject activeInspect;
    private ItemInstance item;

    private static Dictionary<ItemInstance, ItemInspectGui> openItemInspects = new Dictionary<ItemInstance, ItemInspectGui>();

    public GuiWindow Window { get; private set; }

    private void Awake()
    {
        genericItemInspect.gameObject.SetActive(false);
        weaponItemInspect.gameObject.SetActive(false);

        Window = GetComponent<GuiWindow>();
        Window.OnClose += OnClose;
    }

    public void Open(ItemInstance item)
    {
        if (openItemInspects.ContainsKey(item))
        {
            openItemInspects[item].Window.BringToFront();
            Destroy(gameObject);
        }
        else
        {
            this.item = item;

            if (item as WeaponInstance != null)
            {
                activeInspect = weaponItemInspect.gameObject;
                weaponItemInspect.gameObject.SetActive(true);
                weaponItemInspect.SetItem(item);
            }
            else
            {
                activeInspect = genericItemInspect.gameObject;
                genericItemInspect.gameObject.SetActive(true);
                genericItemInspect.SetItem(item);
            }

            openItemInspects.Add(item, this);
        }
    }

    private void OnClose()
    {
        if (activeInspect)
            activeInspect.gameObject.SetActive(false);
        activeInspect = null;

        if (item != null)
            openItemInspects.Remove(item);
        item = null;
    }
}
