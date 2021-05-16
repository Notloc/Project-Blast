using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Notloc.Terrain
{
    public struct MeshData
    {
        private int triIndex;
        public int[] triangles;
        public Vector3[] vertices;
        public Vector2[] uvs;

        public MeshData(int meshWidth, int meshHeight)
        {
            vertices = new Vector3[meshWidth * meshHeight];
            triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
            uvs = new Vector2[meshWidth * meshHeight];
            triIndex = 0;
        }

        public void AddTriangle(int a, int b, int c)
        {
            triangles[triIndex] = a;
            triangles[triIndex + 1] = b;
            triangles[triIndex + 2] = c;
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
}