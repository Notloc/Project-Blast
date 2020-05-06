using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TerrainNoise
{
    public enum NormalizeMode { Local, Global }

    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset, NormalizeMode normalizeMode)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        float maxPossibleHeight = 0f;
        float amplitude = 1;

        System.Random rng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            octaveOffsets[i] = new Vector2(
                rng.Next(-100000, 100000) + offset.x,
                rng.Next(-100000, 100000) + offset.y
            );

            maxPossibleHeight += amplitude;
            amplitude = amplitude * persistance;
        }

        float maxLocalNoiseVal = float.MinValue;
        float minLocalNoiseVal = float.MaxValue;

        float halfW = mapWidth / 2f;
        float halfH = mapHeight / 2f;

        for (float y = 0; y < mapHeight; y++)
        {
            for (float x = 0; x < mapWidth; x++)
            {

                amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float xCoord = (x - halfW + octaveOffsets[i].x) / scale * frequency;
                    float yCoord = (y - halfH + octaveOffsets[i].y) / scale * frequency;

                    float val = Mathf.PerlinNoise(xCoord, yCoord) * 2f - 1f;
                    noiseHeight += val * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;

                    if (val > maxLocalNoiseVal)
                        maxLocalNoiseVal = val;
                    if (val < minLocalNoiseVal)
                        minLocalNoiseVal = val;
                }

                noiseMap[(int)x, (int)y] = noiseHeight;
            }
        }



        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++)
            {
                if (normalizeMode == NormalizeMode.Local) {
                    noiseMap[x, y] = Mathf.InverseLerp(minLocalNoiseVal, maxLocalNoiseVal, noiseMap[x, y]);
                }
                else {
                    noiseMap[x, y] = Mathf.Clamp((noiseMap[x, y] + 1f) / maxPossibleHeight, 0f, float.MaxValue);
                }
            }
        }
        return noiseMap;
    }
}
