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
        rigidbody.mass = item.GetBase().GetWeight();
    }

    public void Interact(Player player)
    {
        var inventory = player.GetInventory();
        if (inventory.Add(item))
            Destroy(this.gameObject);
    }
}
