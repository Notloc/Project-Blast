using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameObject 
{
    string name { get; }
    Transform transform { get; }
    GameObject gameObject { get; }

    T GetComponent<T>();
    T GetComponentInParent<T>();
    T[] GetComponentsInParent<T>();
    T GetComponentInChildren<T>();
    T[] GetComponentsInChildren<T>();
}
