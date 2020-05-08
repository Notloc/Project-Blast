using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TerrainSettings : UpdatableData
{
    [Header("Noise Generation Settings")]
    public Vector2Int terrainSize = Vector2Int.one;
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
    public List<TerrainRegion> regions = null;

    [Header("Mesh Settings")]
    public float heightScale = 100f;
    public AnimationCurve heightCurve = null;

    public Vector2Int physicalSize { get { return terrainSize - Vector2Int.one; } }

    protected override void OnValidate()
    {
        if (terrainSize.x < 1)

            terrainSize.x = 1;
        if (terrainSize.y < 1)
            terrainSize.y = 1;

        if (octaves < 0)
            octaves = 0;

        if (lacunarity < 1)
            lacunarity = 1;

        base.OnValidate();
    }
}

[System.Serializable]
public struct TerrainRegion
{
    public string name;
    public float height;
    public Color color;
}
