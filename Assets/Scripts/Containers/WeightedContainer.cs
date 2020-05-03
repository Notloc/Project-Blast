using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedContainer : Container
{
    [SerializeField] float weightLimit;


    protected override bool CanAdd(List<ItemCount> items)
    {
        return false && base.CanAdd(items);
    }
}
