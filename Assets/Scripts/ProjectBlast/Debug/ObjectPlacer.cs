using Notloc.Terrain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Notloc.Terrain.EndlessTerrain;

namespace ProjectBlast.Debug
{
    public class ObjectPlacer : MonoBehaviour
    {
        [SerializeField] float entitiesPerChunk = 50f;

        [SerializeField] LayerMask terrainLayers = 0;

        [SerializeField] float rayStartHeight = 800f;
        [SerializeField] float rayLength = 1000f;

        [SerializeField] GameObject prefab = null;
        [SerializeField] EndlessTerrain endlessTerrain = null;

        const int maxAttempts = 500;

        private void Awake()
        {
            endlessTerrain.OnChunkCreated += PopulateChunk;
        }

        public void PopulateChunk(TerrainChunk chunk)
        {
            bool activeState = chunk.gameObject.activeSelf;
            if (!activeState)
                chunk.gameObject.SetActive(true);

            float scale = endlessTerrain.Scale;
            Vector2 areaSize = (Vector2)chunk.terrainData.settings.unitSize * scale;

            int successes = 0;
            for (int i = maxAttempts; i > 0; i--)
            {
                if (RandomlySpawnObject(chunk.gameObject, areaSize))
                    successes++;

                if (successes >= entitiesPerChunk)
                    break;
            }

            if (!activeState)
                chunk.gameObject.SetActive(false);
        }

        private bool RandomlySpawnObject(GameObject chunk, Vector2 areaSize)
        {
            Vector3 rayStart = chunk.transform.position + new Vector3(
                Random.Range(0f, areaSize.x),
                rayStartHeight,
                Random.Range(0f, areaSize.y)
            );

            RaycastHit hit;
            if (Physics.Raycast(rayStart, Vector3.down, out hit, rayLength, terrainLayers))
            {
                var newEntity = Instantiate(prefab, chunk.transform);
                newEntity.transform.position = hit.point;
                newEntity.transform.rotation = Quaternion.Euler(
                    new Vector3(
                        Random.Range(-15f, 15f),
                        Random.Range(0f, 360f),
                        Random.Range(-20f, 20f)
                    )
                );

                return true;
            }
            return false;
        }
    }
}