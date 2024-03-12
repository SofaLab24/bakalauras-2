using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    List<Transform> baseWalkPoints;
    public List<List<Transform>> walkPoints;
    public GameObject enemy;
    public float spawnInterval = 5f;

    public void Start()
    {
        walkPoints = new List<List<Transform>>
        {
            baseWalkPoints
        };
        StartCoroutine(SpawnEnemies());
    }
    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            enemy.GetComponent<EnemyController>().walkPoints = walkPoints[Random.Range(0, walkPoints.Count)];
            Instantiate(enemy, transform.position, transform.rotation);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    public void AddWalkPoint(Vector3 pos, int pathIndex)
    {
        Debug.Log("Adding path to index: " +  pathIndex);
        GameObject objToSpawn = new GameObject();
        objToSpawn.transform.position = new Vector3(pos.x, pos.y + 1, pos.z);
        walkPoints[pathIndex].Add(Instantiate(objToSpawn).transform);
    }
    public void AddNewPath(Vector3 pos, int newIndex, int originalIndex)
    {
        Debug.Log("Copying index " + originalIndex + " list into index " +  newIndex);
        walkPoints.Add(new List<Transform>(walkPoints[originalIndex]));
        AddWalkPoint(pos, newIndex);
    }
}
