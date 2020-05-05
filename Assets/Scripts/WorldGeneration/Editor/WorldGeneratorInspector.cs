using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WorldGenerator))]
public class WorldGeneratorInspector : Editor
{
    WorldGenerator generator;

    private void OnEnable()
    {
        generator = (WorldGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Generate"))
        {
            generator.Generate();
        }

        base.OnInspectorGUI();


        if (generator.AutoUpdate)
        {
            generator.Generate();
        }
    }
}
