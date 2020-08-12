using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedContainer : Container
{
    [SerializeField] float weightLimit = 0f;
    [SerializeField] float weight = 0f;

    protected override bool CanAdd(List<ContainerItem> items)
    {
        if (weightLimit <= 0f)
            return base.CanAdd(items);

        return CheckWeight(items) && base.CanAdd(items);
    }

    protected bool CheckWeight(List<ContainerItem> items)
    {
        float additionWeight = 0f;
        foreach (var cItem in items)
        {
            //additionWeight += cItem.item.GetWeight();
        }
        return weight + additionWeight < weightLimit;
    }
}
