using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GenericItemInspectGui : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI itemName = null;
    [SerializeField] TextMeshProUGUI itemDescription = null;

    ItemInstance item;
    
    public void SetItem(ItemInstance item)
    {
        this.item = item;

        itemName.text = item.Name;
        itemDescription.text = item.Description;
    }

    private void OnDisable()
    {
        this.item = null;
    }
}
