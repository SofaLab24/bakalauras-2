using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    List<Transform> baseWalkPoints;
    public List<List<Transform>> walkPoints;
    public List<EnemyController> enemies;
    public float spawnInterval = 2f;
    public int waveNumber;
    public int waveWorthMultiplier = 20;
    public float waveInflation = 1.05f;
    public float spawnRateDecrease = 0.95f;
    public int additionalHealthPerWaveCount = 5;

    List<EnemyController> currentWave;
    OverlayController overlayController;

    const string HIGHSCORE = "HIGHSCORE";

    public void Start()
    {
        overlayController = FindFirstObjectByType<OverlayController>();
        walkPoints = new List<List<Transform>>
        {
            baseWalkPoints
        };
        waveNumber = 0;
    }
    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < currentWave.Count; i++)
        {
            currentWave[i].walkPoints = walkPoints[Random.Range(0, walkPoints.Count)];
            EnemyController enemy = Instantiate(currentWave[i], new Vector3(transform.position.x, transform.position.y + currentWave[i].heightOffset, transform.position.z), Quaternion.identity);
            int additionalHealth = Mathf.FloorToInt((currentWave.Count * 1f) / additionalHealthPerWaveCount);
            enemy.health += additionalHealth;
            yield return new WaitForSeconds(spawnInterval);
        }
        spawnInterval *= spawnRateDecrease;
        overlayController.ShowNextWaveButton();
    }
    List<EnemyController> EnemiesToSpawn(int waveCount)
    {
        List<EnemyController> enemiesArray = new List<EnemyController>();
        while (waveCount > 0)
        {
            EnemyController enemyToAdd = enemies[Random.Range(0, enemies.Count)];
            if (enemyToAdd.waveWorth < waveCount + enemies[0].waveWorth)
            {
                enemiesArray.Add(enemyToAdd);
                waveCount -= enemyToAdd.waveWorth;
            }
        }
        return enemiesArray;
    }
    public void GenerateNextWave()
    {
        waveNumber++;
        currentWave = EnemiesToSpawn(waveNumber * waveWorthMultiplier);
        waveWorthMultiplier = Mathf.FloorToInt(waveWorthMultiplier * waveInflation);
        StartCoroutine(SpawnEnemies());
    }
    public void AddWalkPoint(Vector3 pos, int pathIndex)
    {
        Debug.Log("Adding path to index: " +  pathIndex);
        GameObject objToSpawn = new GameObject();
        objToSpawn.transform.position = new Vector3(pos.x, pos.y, pos.z);
        walkPoints[pathIndex].Add(Instantiate(objToSpawn).transform);
    }
    public void AddNewPath(Vector3 pos, int newIndex, int originalIndex)
    {
        Debug.Log("Copying index " + originalIndex + " list into index " +  newIndex);
        walkPoints.Add(new List<Transform>(walkPoints[originalIndex]));
        AddWalkPoint(pos, newIndex);
    }
    public void DeathEvent()
    {
        int currentHighscore = PlayerPrefs.GetInt(HIGHSCORE);
        Debug.Log("Player prefs: " + currentHighscore + "; current: " + waveNumber);
        if (currentHighscore < waveNumber)
        {
            PlayerPrefs.SetInt(HIGHSCORE, waveNumber);
        }
    }
}
