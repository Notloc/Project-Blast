using ProjectBlast.Items.Containers;
using ProjectBlast.Items.Containers.Gui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBlast.Debugging
{
    public class DebugContainerGui : MonoBehaviour
    {
        Container container;
        [SerializeField] ContainerGui containerView = null;
        [SerializeField] List<ItemBase> items = null;
        [SerializeField] int width, height;

        private void Start()
        {
            container = new Container(width, height);
            
            foreach (ItemBase item in items)
            {
                container.AddItem(new ItemInstance(item));
            }

            containerView.SetContainer(container);
        }
    }
}