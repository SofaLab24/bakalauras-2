using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    List<Transform> baseWalkPoints;
    public List<List<Transform>> walkPoints;
    public List<EnemyController> enemies;
    public float spawnInterval = 2f;
    public int waveNumber;
    public int waveCoinMultiplier = 20;

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
            Instantiate(currentWave[i], new Vector3(transform.position.x, transform.position.y + currentWave[i].heightOffset, transform.position.z), Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
        overlayController.ShowNextWaveButton();
    }
    List<EnemyController> EnemiesToSpawn(int moneyCount)
    {
        List<EnemyController> enemiesArray = new List<EnemyController>();
        while (moneyCount > 0)
        {
            EnemyController enemyToAdd = enemies[Random.Range(0, enemies.Count)];
            if (enemyToAdd.moneyWorth < moneyCount + enemies[0].moneyWorth)
            {
                enemiesArray.Add(enemyToAdd);
                moneyCount -= enemyToAdd.moneyWorth;
            }
        }
        return enemiesArray;
    }
    public void GenerateNextWave()
    {
        waveNumber++;
        currentWave = EnemiesToSpawn(waveNumber * waveCoinMultiplier);
        StartCoroutine(SpawnEnemies());
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
