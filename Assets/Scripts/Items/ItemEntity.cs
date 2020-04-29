using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntity : MonoBehaviour
{
    [SerializeField] new Rigidbody rigidbody = null;
    private GameObject item3D;

    public Rigidbody Rigidbody { get { return rigidbody; } }
    public void Initialize(Item item)
    {
        item3D = Instantiate(item.GetModel(), this.transform);
        rigidbody.mass = item.GetBase().GetWeight();
    }
}
