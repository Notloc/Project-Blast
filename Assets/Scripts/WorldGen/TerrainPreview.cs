using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class TerrainPreview : MonoBehaviour
{
    [SerializeField] TerrainGenerator terrainGenerator = null;
    [SerializeField] new Renderer renderer = null;
    [SerializeField] MeshFilter meshFilter = null;

    [SerializeField] Vector2Int terrainSize = Vector2Int.one;
    [SerializeField] TerrainSettings terrainSettings = null;

    [SerializeField] DisplayMode displayMode = DisplayMode.COLOR;
    private enum DisplayMode
    {
        HEIGHT_MAP,
        COLOR
    }


    public void GeneratePreview()
    {
        TerrainData data = terrainGenerator.GenerateTerrainData(terrainSettings, terrainSize);

        Texture2D texture = new Texture2D(data.size.x, data.size.y);
        texture.filterMode = FilterMode.Point;
        texture.SetPixels(data.colorMap);
        texture.Apply();

        renderer.sharedMaterial.SetTexture("_MainTex", texture);
        meshFilter.mesh = data.meshData.CreateMesh();
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
