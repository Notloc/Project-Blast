using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator
{
    public static Texture2D TextureFromColorMap(Color[] colorMap, int width, int height)
    {
        Texture2D tex = new Texture2D(width, height);
        tex.filterMode = FilterMode.Point;
        
        Color[] pixels = new Color[width * height];
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++)
            {
                int index = (y * width) + x;
                pixels[index] = colorMap[index];
            }
        }

        tex.SetPixels(pixels);
        tex.Apply();
        return tex;
    }

    public static Texture2D TextureFromHeightMap(float[,] heightMap)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        Texture2D tex = new Texture2D(width, height);
        Color32[] pixels = new Color32[width * height];

        Color32 white = Color.white;
        Color32 black = Color.black;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = (y * width) + x;
                pixels[index] = Color32.Lerp(Color.black, Color.white, heightMap[x, y]);
            }
        }

        tex.filterMode = FilterMode.Point;
        tex.SetPixels32(pixels);
        tex.Apply();

        return tex;
    }
}