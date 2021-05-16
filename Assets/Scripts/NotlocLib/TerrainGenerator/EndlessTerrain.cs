using Notloc.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Notloc.Terrain
{
    public class EndlessTerrain : MonoBehaviour
    {
        [SerializeField] TerrainGenerator terrainGenerator = null;
        [SerializeField] TerrainSettings terrainSettings = null;
        [SerializeField] Material mapMaterial = null;

        [SerializeField] Transform observer = null;
        [SerializeField] float viewDistance = 500;
        [SerializeField] float terrainScale = 1f;

        List<TerrainChunk> visibleChunks = new List<TerrainChunk>();
        private Dictionary<Vector2Int, TerrainChunk> terrainChunks = new Dictionary<Vector2Int, TerrainChunk>();
        public event Action<TerrainChunk> OnChunkCreated;
        public float Scale { get { return terrainScale; } }

        private int maxChunkVisionRange;
        private void Start()
        {
            maxChunkVisionRange = Mathf.CeilToInt(viewDistance / terrainSettings.unitSize.x);
        }

        void Update()
        {
            UpdateChunks();
        }

        private void UpdateChunks()
        {
            Vector3 viewPosition = observer.position;
            Vector2Int worldSize = terrainSettings.unitSize;

            Vector3 localScale = transform.localScale;

            Vector2Int playerCoord = new Vector2Int(
                Mathf.FloorToInt(viewPosition.x / terrainScale / worldSize.x),
                Mathf.FloorToInt(viewPosition.z / terrainScale / worldSize.y)
            );

            HashSet<TerrainChunk> chunksToUpdate = new HashSet<TerrainChunk>(visibleChunks);
            visibleChunks.Clear();

            for (int y = -maxChunkVisionRange; y <= maxChunkVisionRange; y++)
            {
                for (int x = -maxChunkVisionRange; x <= maxChunkVisionRange; x++)
                {
                    Vector2Int chunkCoord = playerCoord + new Vector2Int(x, y);

                    if (!terrainChunks.ContainsKey(chunkCoord))
                    {
                        var chunk = new TerrainChunk(this, terrainGenerator, terrainSettings, chunkCoord, mapMaterial, transform);
                        terrainChunks.Add(chunkCoord, chunk);
                        chunksToUpdate.Add(chunk);
                    }
                    else
                    {
                        chunksToUpdate.Add(terrainChunks[chunkCoord]);
                    }
                }
            }

            foreach (var chunk in chunksToUpdate)
                if (chunk.UpdateChunk(viewPosition, viewDistance))
                    visibleChunks.Add(chunk);
        }


        public class TerrainChunk
        {
            public GameObject gameObject { get; private set; }
            public MeshFilter meshFilter { get; private set; }
            public MeshRenderer renderer { get; private set; }
            public MeshCollider meshCollider { get; private set; }
            public TerrainData terrainData { get; private set; }

            private EndlessTerrain endlessTerrain;

            public TerrainChunk(EndlessTerrain endlessTerrain, TerrainGenerator terrainGenerator, TerrainSettings terrainSettings, Vector2Int chunkCoord, Material material, Transform parent)
            {
                this.endlessTerrain = endlessTerrain;

                gameObject = new GameObject("TerrainChunk");
                gameObject.layer = LayerMask.NameToLayer("Terrain");
                var meshObj = new GameObject("Mesh");
                meshObj.transform.SetParent(gameObject.transform);
                meshObj.layer = LayerMask.NameToLayer("Terrain");

                renderer = meshObj.AddComponent<MeshRenderer>();
                meshFilter = meshObj.AddComponent<MeshFilter>();
                meshCollider = meshObj.AddComponent<MeshCollider>();

                renderer.material = material;
                Vector2 offset = Vector2.Scale(chunkCoord, terrainSettings.unitSize);
                terrainGenerator.RequestTerrainData(terrainSettings, offset, ReceiveTerrainData);

                gameObject.transform.parent = parent;
                gameObject.transform.localPosition = offset.ToVector3Z() * endlessTerrain.Scale;
                meshObj.transform.localScale = Vector3.one * endlessTerrain.Scale;
            }

            private void ReceiveTerrainData(TerrainData terrainData)
            {
                this.terrainData = terrainData;

                Texture2D texture = new Texture2D(terrainData.size.x, terrainData.size.y);
                texture.filterMode = FilterMode.Point;
                texture.SetPixels(terrainData.colorMap);
                texture.Apply();
                renderer.material.SetTexture("_MainTex", texture);

                Mesh mesh = terrainData.meshData.CreateMesh();
                meshFilter.mesh = mesh;
                meshCollider.sharedMesh = mesh;

                endlessTerrain.OnChunkCreated(this);
            }

            public bool UpdateChunk(Vector3 viewerPos, float viewDistance)
            {
                bool visible = IsVisible(viewerPos, viewDistance);
                gameObject.SetActive(visible);
                return visible;
            }

            private bool IsVisible(Vector3 viewPosition, float viewDistance)
            {
                float scale = endlessTerrain.Scale;
                viewDistance *= scale;
                Vector2 size = (Vector2)terrainData.size * scale;
                Bounds bounds = new Bounds(gameObject.transform.position + new Vector3(size.x, 0f, size.y) / 2f, new Vector3(size.x, 5f, size.y));

                Vector3 testPoint = bounds.ClosestPoint(viewPosition.Flatten());
                return Vector3.Distance(testPoint, viewPosition) < viewDistance;
            }
        }

    }
}