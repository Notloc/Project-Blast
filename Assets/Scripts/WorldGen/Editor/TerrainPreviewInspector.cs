using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainPreview))]
public class TerrainPreviewInspector : Editor
{
    TerrainPreview preview;
    private void OnEnable()
    {
        preview = (TerrainPreview)target;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Generate"))
        {
            preview.GeneratePreview();
        }

        base.OnInspectorGUI();
    }
}
