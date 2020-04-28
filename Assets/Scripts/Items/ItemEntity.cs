using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntity : MonoBehaviour
{
    [SerializeField] new Rigidbody rigidbody;
    private GameObject item3D;
    public void Initialize(Item item)
    {
        item3D = Instantiate(item.GetModel(), this.transform);
        rigidbody.mass = item.GetBase().GetWeight();
    }
}
