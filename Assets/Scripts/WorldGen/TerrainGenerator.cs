using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class and related classes are based on this tutorial series
// https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3
public class TerrainGenerator : MonoBehaviour
{
    enum DisplayMode
    {
        HEIGHT_MAP,
        COLOR
    }

    public bool AutoUpdate = false;

    [Header("Noise Generation Settings")]
    [SerializeField] Vector2Int terrainResolution = Vector2Int.one;
    [SerializeField] float noiseScale = 1f;
    [Range(0, 15)]
    [SerializeField] int octaves = 3;
    [Range(0f,1f)]
    [SerializeField] float persistance = 1f;
    [SerializeField] float lacunarity = 1f;    
    [SerializeField] Vector2 noiseOffset = Vector2.zero;
    [SerializeField] int noiseSeed = 9999;
    [SerializeField] TerrainNoise.NormalizeMode normalizeMode = TerrainNoise.NormalizeMode.Global;

    [Header("Texture Settings")]
    [SerializeField] new Renderer renderer = null;
    [SerializeField] DisplayMode displayMode = DisplayMode.COLOR;
    [SerializeField] List<TerrainRegion> regions = null;
    
    [Header("Mesh Settings")]
    [SerializeField] float heightScale = 100f;
    [SerializeField] AnimationCurve heightCurve = null;
    [SerializeField] MeshFilter meshFilter = null;
    [SerializeField] MeshCollider meshCollider = null;

    private void Start()
    {
        Generate();
    }

    public void Generate()
    {
        int width = terrainResolution.x;
        int height = terrainResolution.y;

        float[,] heightMap = TerrainNoise.GenerateNoiseMap(width, height, noiseSeed, noiseScale, octaves, persistance, lacunarity, noiseOffset, normalizeMode);

        Color[] colorMap = new Color[width * height];
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++)
            {
                int index = width * y + x;
                colorMap[index] = GetColor(heightMap[x,y]);
            }
        }

        Texture2D tex;
        if (displayMode == DisplayMode.COLOR)
            tex = TextureGenerator.TextureFromColorMap(colorMap, width, height);
        else
            tex = TextureGenerator.TextureFromHeightMap(heightMap);

        renderer.sharedMaterial.SetTexture("_MainTex", tex);

        var mesh = MeshGenerator.GenerateMesh(heightMap, heightScale, heightCurve);
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
    }

    private Color32 GetColor(float height)
    {
        Color col = Color.black;

        for (int i = 0; i < regions.Count; i++)
            if (regions[i].height <= height)
                col = regions[i].color;

        return col;
    }

    private void OnValidate()
    {
        if (terrainResolution.x < 1)
            terrainResolution.x = 1;
        if (terrainResolution.y < 1)
            terrainResolution.y = 1;

        if (octaves < 0)
            octaves = 0;

        if (lacunarity < 1)
            lacunarity = 1;
    }
}
[System.Serializable]
public struct TerrainRegion
{
    public string name;
    public float height;
    public Color color;
}
