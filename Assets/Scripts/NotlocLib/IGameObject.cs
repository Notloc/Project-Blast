using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utility interface meant to be extended by other interfaces.
/// Gives access to common gameobject properties directly from the interface reference.
/// </summary>
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
