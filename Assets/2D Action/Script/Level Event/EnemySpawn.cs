using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ai;
using Stats;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] GameObject EnemyPrefab;
    [SerializeField] List<WaveInfo> waves;
    [SerializeField] Transform target;
    public LevelArena levelArena;
    public int enemySpawnNumber = 0;

    [SerializeField] private int currentWave = 0;
    private int spawnedEnemies = 0;
    private float spawnTimer = 0f;

    [SerializeField] float spawnCoolDown = 1.5f;
    public bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive || currentWave >= waves.Count) return;

        WaveInfo wave = waves[currentWave];

        if (spawnedEnemies < wave.enemyCount)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnCoolDown)
            {
                spawnTimer = 0f;
                SpawnEnemy();
                spawnedEnemies++;
            }
        }
        else if (enemySpawnNumber <= 0)
        {
            StartNextWave();
        }
    }
    private void SpawnEnemy()
    {
        WaveInfo wave = waves[currentWave];
        Vector3 spawnPos = wave.spawnPoint != null ? wave.spawnPoint.position : transform.position;

        GameObject enemy = Instantiate(EnemyPrefab, spawnPos, Quaternion.identity);
        enemy.GetComponent<AiAgent>().target = target;
        enemy.GetComponent<AiStat>().partOfSpawnArena = this;
        enemySpawnNumber ++;
    }

    public void StartNextWave()
    {
        if (currentWave < waves.Count - 1)
        {
            currentWave++;
            spawnedEnemies = 0;
            spawnTimer = 0f;
        }
        else
        {
            Debug.Log("All waves completed!");
            levelArena.activespawnpoint--;
            isActive = false;
        }
    } 
}



