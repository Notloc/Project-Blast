using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour
{
    [SerializeField] TerrainGenerator terrainGenerator = null;
    [SerializeField] TerrainSettings terrainSettings = null;
    [SerializeField] float viewDistance = 500;
    [SerializeField] Transform observer = null;
    [SerializeField] Material mapMaterial = null;

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

    List<TerrainChunk> visibleChunks = new List<TerrainChunk>();
    private void UpdateChunks()
    {
        Vector3 viewPosition = observer.position;
        Vector2Int worldSize = terrainSettings.physicalSize;

        Vector3 localScale = transform.localScale;

        Vector2Int playerCoord = new Vector2Int(
            Mathf.FloorToInt((viewPosition.x / localScale.x) / worldSize.x),
            Mathf.FloorToInt((viewPosition.z / localScale.z) / worldSize.y)
        );

        HashSet<TerrainChunk> chunksToUpdate = new HashSet<TerrainChunk>(visibleChunks);
        visibleChunks.Clear();

        for (int y = -chunksVisible; y <= chunksVisible; y++) {
            for (int x = -chunksVisible; x <= chunksVisible; x++)
            {
                Vector2Int chunkCoord = playerCoord + new Vector2Int(x, y);

                if (!terrainChunks.ContainsKey(chunkCoord))
                {
                    var chunk = new TerrainChunk(terrainGenerator, terrainSettings, chunkCoord, mapMaterial, transform);
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

}

public class TerrainChunk
{
    private GameObject gameObject;
    private MeshFilter meshFilter;
    private MeshRenderer renderer;
    private MeshCollider meshCollider;

    private TerrainData terrainData;

    public TerrainChunk(TerrainGenerator terrainGenerator, TerrainSettings terrainSettings, Vector2Int chunkCoord, Material material, Transform parent)
    {
        gameObject = new GameObject("TerrainChunk");
        gameObject.layer = LayerMask.NameToLayer("Terrain");
        renderer = gameObject.AddComponent<MeshRenderer>();
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshCollider = gameObject.AddComponent<MeshCollider>();

        renderer.material = material;
        Vector2 offset = Vector2.Scale(chunkCoord, terrainSettings.physicalSize);
        terrainGenerator.RequestTerrainData(terrainSettings, offset, ReceiveTerrainData);

        gameObject.transform.parent = parent;
        gameObject.transform.localPosition = offset.ToVector3Z();
        gameObject.transform.localScale = Vector3.one;
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
    }

    public bool UpdateChunk(Vector3 viewerPos, float viewDistance)
    {
        bool visible = IsVisible(viewerPos, viewDistance);
        gameObject.SetActive(visible);
        return visible;
    }

    private bool IsVisible(Vector3 viewPosition, float viewDistance)
    {
        float scale = gameObject.transform.lossyScale.x;
        viewDistance *= scale;
        Vector2 size = ((Vector2)terrainData.size) * scale;
        Bounds bounds = new Bounds(gameObject.transform.position + (new Vector3(size.x, 0f, size.y)/2f), new Vector3(size.x, 5f, size.y));

        Vector3 testPoint = bounds.ClosestPoint(viewPosition.Flatten());
        return Vector3.Distance(testPoint, viewPosition) < viewDistance;
    }
}