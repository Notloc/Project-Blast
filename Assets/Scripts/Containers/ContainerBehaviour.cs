using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerBehaviour : MonoBehaviour
{
    [SerializeField] ContainerContents startingContents = null;
    [Space]
    [SerializeField] ContainerType type = ContainerType.BASIC;
    [SerializeField] Container container = new Container();

    void Start()
    {
        if (type == ContainerType.BASIC)
            container = new Container();
        else if (type == ContainerType.WEIGHTED)
            container = new WeightedContainer();
        else if (type == ContainerType.INVENTORY)
            ;// container = new InventoryContainer();

        if (startingContents)
            container.Init(startingContents);
    }

    public Container GetContainer()
    {
        return container;
    }

    public void EmptyIntoWorld()
    {
        if (container == null)
            return;

        var items = container.Items;
        var removed = new List<ContainerItem>(items.Values);
        if (container.Remove(removed))
        {
            foreach (var itemC in removed)
                for (int i = 0; i < itemC.count; i++)
                    Game.Instance.Factories.ItemEntityFactory.CreateItemEntity(itemC.item, transform.position + Vector3.up);
        }
    }
}
