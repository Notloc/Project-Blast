using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour
{
    [SerializeField] TerrainSettings terrainSettings = null;
    [SerializeField] float viewDistance = 500;
    [SerializeField] Transform observer = null;
    [SerializeField] Material mapMaterial;

    private Dictionary<Vector2Int, TerrainChunk> terrainChunks = new Dictionary<Vector2Int, TerrainChunk>();

    private int chunksVisible = 1;
    private void Start()
    {
        chunksVisible = Mathf.CeilToInt(viewDistance / terrainSettings.physicalSize.x);
    }

    void Update()
    {
        UpdateChunks();
    }


    private void UpdateChunks()
    {
        Vector3 viewPosition = observer.position;
        Vector2Int worldSize = terrainSettings.physicalSize;

        Vector3 localScale = transform.localScale;

        Vector2Int playerCoord = new Vector2Int(
            Mathf.FloorToInt((viewPosition.x / localScale.x) / worldSize.x),
            Mathf.FloorToInt((viewPosition.z / localScale.z) / worldSize.y)
        );

        for (int y = -chunksVisible; y <= chunksVisible; y++) {
            for (int x = -chunksVisible; x <= chunksVisible; x++)
            {
                Vector2Int chunkCoord = playerCoord + new Vector2Int(x, y);

                if (terrainChunks.ContainsKey(chunkCoord))
                    terrainChunks[chunkCoord].UpdateChunk();
                else
                {
                    var chunk = new TerrainChunk(terrainSettings, chunkCoord, mapMaterial, transform);
                    terrainChunks.Add(chunkCoord, chunk);
                    chunk.UpdateChunk();
                }

                
            }
        }
        
    }

}

public class TerrainChunk
{
    private GameObject chunk;

    public TerrainChunk(TerrainSettings terrainSettings, Vector2Int chunkCoord, Material mat, Transform parent)
    {
        Vector2 offset = Vector2.Scale(chunkCoord, terrainSettings.physicalSize);
        TerrainData terrainData = TerrainGenerator.GenerateTerrainData(terrainSettings, offset);

        GameObject chunk = new GameObject("TerrainChunk");
        var renderer = chunk.AddComponent<MeshRenderer>();
        var meshFilter = chunk.AddComponent<MeshFilter>();
        var meshCollider = chunk.AddComponent<MeshCollider>();

        renderer.material = mat;
        renderer.material.SetTexture("_MainTex", terrainData.texture);
        meshFilter.mesh = terrainData.mesh;
        meshCollider.sharedMesh = terrainData.mesh;

        chunk.transform.parent = parent;
        chunk.transform.localPosition = offset.ToVector3Z();
        chunk.transform.localScale = Vector3.one;
    }

    public void UpdateChunk()
    {

    }
}