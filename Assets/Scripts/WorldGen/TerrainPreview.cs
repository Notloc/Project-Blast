using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class TerrainPreview : MonoBehaviour
{
    [SerializeField] new Renderer renderer = null;
    [SerializeField] MeshFilter meshFilter = null;

    [SerializeField] Vector2Int terrainSize = Vector2Int.one;
    [SerializeField] TerrainSettings terrainSettings = null;

    public void GeneratePreview()
    {
        TerrainData data = TerrainGenerator.GenerateTerrainData(terrainSettings, terrainSize);
        renderer.sharedMaterial.SetTexture("_MainTex", data.texture);
        meshFilter.mesh = data.mesh;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (terrainSettings)
        {
            terrainSettings.OnDataUpdated -= GeneratePreview;
            terrainSettings.OnDataUpdated += GeneratePreview;
        }

        UnityEditor.EditorApplication.update += UpdatePreview;
    }

    private void UpdatePreview()
    {
        UnityEditor.EditorApplication.update -= UpdatePreview;
        GeneratePreview();
    }
#endif
}
