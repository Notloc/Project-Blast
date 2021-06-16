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
    
    public virtual void SetItem(ItemInstance item)
    {
        this.item = item;

        itemName.text = item.Name;
        itemDescription.text = item.Description;
        itemImage.sprite = item.Sprite;
    }

    private void OnDisable()
    {
        this.item = null;
    }
}
