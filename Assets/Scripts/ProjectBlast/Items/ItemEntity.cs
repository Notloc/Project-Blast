using ProjectBlast.Interaction;
using ProjectBlast.PlayerScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBlast.Game
{
    public class ItemEntity : GrabbableEntity, IInteractable
    {
        private GameObject item3D;
        private Item item;

        public void Init(Item item)
        {
            this.item = item;
            item3D = Instantiate(item.GetModel(), transform);
            rigidbody.mass = item.GetBase().GetWeight();
            name = "Item  - " + item.GetName();
        }

        public void Interact(Player player)
        {
            var inventory = player.GetInventory();
            if (inventory.Add(item))
                Destroy(gameObject);
        }
    }
}