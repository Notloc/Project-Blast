using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntity : GrabbableEntity, IInteractable
{
    private GameObject item3D;
    private Item item;

    public void Init(Item item)
    {
        this.item = item;
        item3D = Instantiate(item.GetModel(), this.transform);
        rigidbody.mass = item.GetItemBase().GetWeight();
        this.name = "Item  - "+item.GetName();

    }

    public void Interact(Player player)
    {
        var inventory = player.Inventory;
        if (inventory.Add(item))
            Destroy(this.gameObject);
    }
}
