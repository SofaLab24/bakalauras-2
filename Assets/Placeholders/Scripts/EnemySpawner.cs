using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<Transform> walkPoints;
    public GameObject enemy;
    public float spawnInterval = 5f;

    public void Start()
    {
        StartCoroutine(SpawnEnemies());
    }
    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            enemy.GetComponent<EnemyController>().walkPoints = walkPoints;
            Instantiate(enemy, transform.position, transform.rotation);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
