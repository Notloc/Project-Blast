using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainGenerator))]
public class WorldGeneratorInspector : Editor
{
    TerrainGenerator generator;

    private void OnEnable()
    {
        generator = (TerrainGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Generate"))
        {
            generator.Generate();
        }

        if(DrawDefaultInspector())
            if (generator.AutoUpdate)
                generator.Generate();
    }
}
