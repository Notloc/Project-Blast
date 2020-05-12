using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerBehaviour : MonoBehaviour
{
    public enum ContentsMode
    {
        PRESET,
        RANDOM
    }
    
    [SerializeField] ContainerType type = ContainerType.BASIC;

    [Header("Contents Options")]
    [SerializeField] ContentsMode contentsMode = ContentsMode.PRESET;
    [SerializeField] ContainerContents startingContents = null;
    [SerializeField] List<ContainerContents> randomContents = null;
    [Space]
    [SerializeField] Container container = new Container();

    void Start()
    {
        if (type == ContainerType.BASIC)
            container = new Container();
        else if (type == ContainerType.WEIGHTED)
            container = new WeightedContainer();
        else if (type == ContainerType.INVENTORY)
            ;// container = new InventoryContainer();

        if (contentsMode == ContentsMode.PRESET)
        {
            if (startingContents)
                container.Init(startingContents);
        }
        else
        {
            if (randomContents.Count > 0)
                container.Init(randomContents[Random.Range(0, randomContents.Count)]);
        }
    }

    public Container GetContainer()
    {
        return container;
    }

    public void EmptyIntoWorld()
    {
        if (container == null || isSpewing)
            return;

        StartCoroutine(SpewItems());
    }

    bool isSpewing = false;
    private IEnumerator SpewItems()
    {
        isSpewing = true;

        var anim = GetComponent<ContainerAnimation>();
        if (anim)
            anim.OpenContainer();


        yield return new WaitForSeconds(0.1f);

        var items = container.Items;
        var removed = new List<ContainerItem>(items.Values);
        if (container.Remove(removed))
        {
            foreach (var itemC in removed)
            {
                for (int i = 0; i < itemC.count; i++)
                {
                    Game.Instance.Factories.ItemEntityFactory.CreateItemEntity(itemC.item, transform.position + Vector3.up);
                    yield return new WaitForFixedUpdate();
                }
            }
        }

        isSpewing = false;
    }

}
