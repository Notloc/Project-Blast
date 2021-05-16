using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(ContainerBehaviour))]
public class ContainerInspector : Editor
{
    private ContainerBehaviour containerBehaviour;
    void OnEnable()
    {
        containerBehaviour = (ContainerBehaviour)target;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Empty Container Into World"))
        {
            containerBehaviour.EmptyIntoWorld();
        }
        base.OnInspectorGUI();
    }
}