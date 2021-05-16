using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Notloc.Terrain
{
    public static class MeshGenerator
    {
        public static MeshData GenerateMesh(float[,] heightMap, float heightScale, AnimationCurve heightCurve)
        {
            int width = heightMap.GetLength(0);
            int height = heightMap.GetLength(1);

            MeshData meshData = new MeshData(width, height);
            int vertIndex = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    meshData.vertices[vertIndex] = new Vector3(x, heightCurve.Evaluate(heightMap[x, y]) * heightScale, y);
                    meshData.uvs[vertIndex] = new Vector2(x / (float)width, y / (float)height);

                    if (x < width - 1 && y < height - 1)
                    {
                        meshData.AddTriangle(vertIndex, vertIndex + width, vertIndex + width + 1);
                        meshData.AddTriangle(vertIndex, vertIndex + width + 1, vertIndex + 1);
                    }

                    vertIndex++;
                }
            }

            return meshData;
        }
    }
}