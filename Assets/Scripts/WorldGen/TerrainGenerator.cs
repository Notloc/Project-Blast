using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;
using System.Dynamic;
using System.Runtime.CompilerServices;
using static MeshGenerator;

// This class and related classes are based on this tutorial series
// https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3
public class TerrainGenerator : MonoBehaviour
{
    public TerrainData GenerateTerrainData(TerrainSettings terrainSettings)
    {
        return GenerateTerrainData(terrainSettings, terrainSettings.terrainSize, terrainSettings.noiseOffset);
    }

    public TerrainData GenerateTerrainData(TerrainSettings terrainSettings, Vector2 offset)
    {
        return GenerateTerrainData(terrainSettings, terrainSettings.terrainSize, offset);
    }

    public TerrainData GenerateTerrainData(TerrainSettings terrainSettings, Vector2Int size)
    {
        return GenerateTerrainData(terrainSettings, size, terrainSettings.noiseOffset);
    }

    public TerrainData GenerateTerrainData(TerrainSettings terrainSettings, Vector2Int size, Vector2 offset)
    {
        int width = size.x;
        int height = size.y;

        AnimationCurve heightCurve = new AnimationCurve(terrainSettings.heightCurve.keys);

        float[,] heightMap = TerrainNoise.GenerateNoiseMap(width, height, terrainSettings.noiseSeed, terrainSettings.noiseScale, terrainSettings.octaves, terrainSettings.persistance, terrainSettings.lacunarity, offset, terrainSettings.normalizeMode);
        Color[] colorMap = CreateColorMap(heightMap, terrainSettings.regions);
        MeshData meshData = MeshGenerator.GenerateMesh(heightMap, terrainSettings.heightScale, heightCurve);

        return new TerrainData(heightMap, colorMap, meshData, size, terrainSettings);
    }


    struct TerrainThreadData {
        public readonly Action<TerrainData> callback;
        public readonly TerrainData terrainData;

        public TerrainThreadData(Action<TerrainData> callback, TerrainData terrainData)
        {
            this.callback = callback;
            this.terrainData = terrainData;
        }
    }
    Queue<TerrainThreadData> callbackQueue = new Queue<TerrainThreadData>();

    public void RequestTerrainData(TerrainSettings settings, Vector2 offset, Action<TerrainData> callback)
    {
        ThreadStart getTerrainData = delegate
        {
            TerrainData terrainData = GenerateTerrainData(settings, offset);
            lock (callbackQueue){
                callbackQueue.Enqueue(new TerrainThreadData(callback, terrainData));
            }
        };
        new Thread(getTerrainData).Start();
        
    }

    private void Update()
    {
        for (int i = callbackQueue.Count; i > 0; i--)
        {
            var terrainThread = callbackQueue.Dequeue();
            terrainThread.callback(terrainThread.terrainData);
        }
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
    public readonly Color[] colorMap;
    public readonly MeshData meshData;
    public readonly Vector2Int size;
    public readonly TerrainSettings settings;

    public TerrainData(float[,] heightMap, Color[] colorMap, MeshData meshData, Vector2Int size, TerrainSettings settings)
    {
        this.heightMap = heightMap;
        this.meshData = meshData;
        this.colorMap = colorMap;
        this.size = size;
        this.settings = settings;
    }
}
