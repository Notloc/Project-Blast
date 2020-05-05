using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public bool AutoUpdate = false;
    [SerializeField] Vector2 offset = Vector2.zero;
    [SerializeField] Vector2Int meshResolution = Vector2Int.one;
    [SerializeField] float scale = 1f;

    [SerializeField] Texture myTex = null;
    [SerializeField] Renderer plane = null;

    [SerializeField] MeshFilter meshFilter;
    [SerializeField] MeshCollider meshCollider;

    [SerializeField] AnimationCurve heightCurve;
    [SerializeField] float heightScale;

    public void Generate()
    {
        Texture2D texture = new Texture2D(meshResolution.x, meshResolution.y);
        var pixels = texture.GetPixels();

        float width = texture.width;
        float height = texture.height;

        float[,] heightMap = new float[(int)width,(int)height];

        for (float y=0; y<height; y++)
        {
            for (float x=0; x <width; x++)
            {
                float xCoord = offset.x + x / width * scale;
                float yCoord = offset.y + y / height * scale;

                float result = Mathf.PerlinNoise(xCoord, yCoord);
                pixels[(texture.width * (int)y) + (int)x] = new Color(result, result, result);

                heightMap[(int)x, (int)y] = result;
            }
        }

        texture.SetPixels(pixels);
        texture.Apply();
        plane.sharedMaterial.SetTexture("_MainTex", texture);


        myTex = texture;
        var mesh = GenerateMesh(heightMap);

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
    }


    public struct MeshData
    {
        private int triIndex;
        public int[] triangles;
        public Vector3[] vertices;
        public Vector2[] uvs;

        public MeshData(int meshWidth, int meshHeight)
        {
            vertices = new Vector3[meshWidth * meshHeight];
            triangles = new int[(meshWidth-1) * (meshHeight-1) * 6];
            uvs = new Vector2[meshWidth * meshHeight];
            triIndex = 0;
        }

        public void AddTriangle(int a, int b, int c)
        {
            triangles[triIndex] = a;
            triangles[triIndex+1] = b;
            triangles[triIndex+2] = c;
            triIndex += 3;
        }

        public Mesh CreateMesh()
        {
            Mesh mesh = new Mesh();

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;

            mesh.RecalculateNormals();
            return mesh;
        }
    }

    private Mesh GenerateMesh(float[,] heightMap)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        MeshData meshData = new MeshData(width, height);
        int vertIndex = 0;

        for(int y=0; y<height; y++)
        {
            for (int x=0; x<width; x++)
            {
                meshData.vertices[vertIndex] = new Vector3(x, heightCurve.Evaluate(heightMap[x, y]) * heightScale, y);
                meshData.uvs[vertIndex] = new Vector2(x / (float)width, y / (float)height);

                if (x < width-1 && y < height - 1)
                {
                    meshData.AddTriangle(vertIndex, vertIndex + width, vertIndex + width + 1);
                    meshData.AddTriangle(vertIndex, vertIndex + width + 1, vertIndex+1);
                }

                vertIndex++;
            }
        }

        return meshData.CreateMesh();
    }

}
