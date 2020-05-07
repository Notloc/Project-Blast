using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TerrainSettings : ScriptableObject
{
    [Header("Noise Generation Settings")]
    public Vector2Int terrainResolution = Vector2Int.one;
    public float noiseScale = 1f;
    [Range(0, 15)]
    public int octaves = 3;
    [Range(0f, 1f)]
    public float persistance = 1f;
    public float lacunarity = 1f;
    public Vector2 noiseOffset = Vector2.zero;
    public int noiseSeed = 9999;
    public TerrainNoise.NormalizeMode normalizeMode = TerrainNoise.NormalizeMode.Global;

    [Header("Texture Settings")]
    public DisplayMode displayMode = DisplayMode.COLOR;
    public List<TerrainRegion> regions = null;

    [Header("Mesh Settings")]
    public float heightScale = 100f;
    public AnimationCurve heightCurve = null;

    public Vector2Int physicalSize { get { return terrainResolution - Vector2Int.one; } }

    public enum DisplayMode
    {
        HEIGHT_MAP,
        COLOR
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
