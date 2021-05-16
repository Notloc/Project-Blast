using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mesh;

namespace Notloc.Terrain
{
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
}