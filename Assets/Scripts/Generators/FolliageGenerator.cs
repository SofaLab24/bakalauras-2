using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FolliageGenerator : MonoBehaviour
{
    [SerializeField]
    Transform camera;
    [SerializeField]
    int renderDistance;

    FolliageChunkGenerator chunkInstance;
    List<Vector2Int> coordsToRemove;

    private void Start()
    {
        chunkInstance = GetComponent<FolliageChunkGenerator>();
        coordsToRemove = new List<Vector2Int>();
    }

    private void Update()
    {
        int cameraChunkX = (int)camera.position.x / (int)(FolliageChunkGenerator.chunkSize.x * FolliageChunkGenerator.folliageSize);
        int cameraChunkY = (int)camera.position.z / (int)(FolliageChunkGenerator.chunkSize.y * FolliageChunkGenerator.folliageSize);
        coordsToRemove.Clear();

        foreach (KeyValuePair<Vector2Int, GameObject> activeChunk in FolliageChunkGenerator.activeChunks)
        {
            coordsToRemove.Add(activeChunk.Key);
        }

        for (int x = cameraChunkX - renderDistance; x <= cameraChunkX + renderDistance; x++)
        {
            for (int y = cameraChunkY - renderDistance; y <= cameraChunkY + renderDistance; y++)
            {
                Vector2Int chunkCoord = new Vector2Int(x, y);
                if (!FolliageChunkGenerator.activeChunks.ContainsKey(chunkCoord))
                {
                    chunkInstance.CreateChunk(chunkCoord);
                }
                coordsToRemove.Remove(chunkCoord); // huh
            }
        }

        foreach (Vector2Int coord in coordsToRemove)
        {
            GameObject chunkToDelete = FolliageChunkGenerator.activeChunks[coord];
            FolliageChunkGenerator.activeChunks.Remove(coord);
            Destroy(chunkToDelete);
        }
    }
}
