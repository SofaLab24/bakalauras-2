using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FolliageChunkGenerator : MonoBehaviour
{
    public GameObject[] folliage;

    public static Dictionary<Vector2Int, int[,]> worldData;
    public static Dictionary<Vector2Int, GameObject> activeChunks;
    public static Vector2Int chunkSize = new Vector2Int(10, 10);

    [Range(0.0f, 0.5f)]
    public float spawnIntensity = 0.062f;

    public float yOffset = -2f;
    public static float folliageSize = 5f;

    private void Start()
    {
        worldData = new Dictionary<Vector2Int, int[,]>();
        activeChunks = new Dictionary<Vector2Int, GameObject>();

        for (int x = -2; x < 2; x++)
        {
            for (int y = -2; y < 2; y++)
            {
                CreateChunk(new Vector2Int(x, y));
            }
        }
    }

    public void CreateChunk(Vector2Int chunkCoord)
    {
        Vector2Int pos = new Vector2Int(chunkCoord.x, chunkCoord.y);
        int[,] dataToApply = worldData.ContainsKey(pos) ? worldData[pos] : GenerateData(pos);

        string chunkName = $"Chunk {chunkCoord.x} {chunkCoord.y}";
        GameObject chunk = new GameObject(chunkName);
        chunk.transform.position = new Vector3(chunkCoord.x * folliageSize * chunkSize.x, yOffset, chunkCoord.y * folliageSize * chunkSize.y);

        for (int x = 0; x < dataToApply.GetLength(0); x++)
        {
            for (int y = 0; y < dataToApply.GetLength(1); y++)
            {
                if (dataToApply[x, y] != -1) 
                {
                    GameObject folToSpawn = Instantiate(folliage[dataToApply[x, y]]);
                    folToSpawn.transform.SetParent(chunk.transform);
                    folToSpawn.transform.localPosition = new Vector3(x * folliageSize, yOffset, y * folliageSize);
                }
            }
        }

        activeChunks.Add(chunkCoord, chunk);
    }

    public int[,] GenerateData(Vector2Int offset)
    {
        int[,] tempData = new int[chunkSize.x, chunkSize.y];

        for (int x = 0; x < chunkSize.x; x++)
        {
            for (int y = 0; y < chunkSize.y; y++)
            {
                if (Random.value < spawnIntensity)
                {
                    int folliageToSpawn = Random.Range(0, folliage.Length);
                    tempData[x, y] = folliageToSpawn;
                    //Instantiate(folliage[folliageToSpawn], new Vector3(x * folliageSize, yOffset, y * folliageSize), Quaternion.identity);
                }
                else
                {
                    tempData[x, y] = -1;
                }
            }
        }

        worldData.Add(offset, tempData);
        return tempData;
    }
}
