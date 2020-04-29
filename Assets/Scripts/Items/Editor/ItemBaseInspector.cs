using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemBase))]
[CanEditMultipleObjects]
public class ItemBaseInspector : Editor
{
    ItemBase item;
    private void OnEnable()
    {
        item = (ItemBase)target;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal("Box");
        {
            GUILayout.Label("Id:");
            GUILayout.Label(item.GetId().ToString());
        }
        GUILayout.EndHorizontal();
        base.OnInspectorGUI();
    }
}
