using ProjectBlast.Items.Containers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GenericItemInspectGui : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI itemName = null;
    [SerializeField] TextMeshProUGUI itemDescription = null;
    [SerializeField] Image itemImage = null; // TODO: replace with 3D preview

    protected ItemInstance item;
    protected IContainer container;

    public virtual void SetItem(ItemInstance item, IContainer container)
    {
        this.item = item;
        this.container = container;

        itemName.text = item.Name;
        itemDescription.text = item.Description;
        itemImage.sprite = item.Sprite;
    }

    private void OnDisable()
    {
        this.item = null;
    }
}
