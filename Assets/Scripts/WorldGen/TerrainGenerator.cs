using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class and related classes are based on this tutorial series
// https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3
public static class TerrainGenerator
{
    public static TerrainData GenerateTerrainData(TerrainSettings terrainSettings)
    {
        return GenerateTerrainData(terrainSettings, terrainSettings.terrainSize, terrainSettings.noiseOffset);
    }

    public static TerrainData GenerateTerrainData(TerrainSettings terrainSettings, Vector2 offset)
    {
        return GenerateTerrainData(terrainSettings, terrainSettings.terrainSize, offset);
    }

    public static TerrainData GenerateTerrainData(TerrainSettings terrainSettings, Vector2Int size)
    {
        return GenerateTerrainData(terrainSettings, size, terrainSettings.noiseOffset);
    }

    public static TerrainData GenerateTerrainData(TerrainSettings terrainSettings, Vector2Int size, Vector2 offset)
    {
        int width = size.x;
        int height = size.y;

        float[,] heightMap = TerrainNoise.GenerateNoiseMap(width, height, terrainSettings.noiseSeed, terrainSettings.noiseScale, terrainSettings.octaves, terrainSettings.persistance, terrainSettings.lacunarity, offset, terrainSettings.normalizeMode);
        Color[] colorMap = CreateColorMap(heightMap, terrainSettings.regions);

        Texture2D tex;
        if (terrainSettings.displayMode == TerrainSettings.DisplayMode.COLOR)
            tex = TextureGenerator.TextureFromColorMap(colorMap, width, height);
        else
            tex = TextureGenerator.TextureFromHeightMap(heightMap);

        Mesh mesh = MeshGenerator.GenerateMesh(heightMap, terrainSettings.heightScale, terrainSettings.heightCurve);

        return new TerrainData(heightMap, mesh, tex);
    }

    private static Color[] CreateColorMap(float[,] heightMap, List<TerrainRegion> regions)
    {
        int width = heightMap.GetLength(0);
        int height= heightMap.GetLength(1);
        Color[] colorMap = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = width * y + x;
                colorMap[index] = GetColor(heightMap[x, y], regions);
            }
        }
        return colorMap;
    }

    private static Color32 GetColor(float height, List<TerrainRegion> regions)
    {
        Color col = Color.black;

        for (int i = 0; i < regions.Count; i++)
            if (regions[i].height <= height)
                col = regions[i].color;

        return col;
    }
}

public struct TerrainData
{
    public readonly float[,] heightMap;
    public readonly Mesh mesh;
    public readonly Texture2D texture;

    public TerrainData(float[,] heightMap, Mesh mesh, Texture2D texture)
    {
        this.heightMap = heightMap;
        this.mesh = mesh;
        this.texture = texture;
    }
}
