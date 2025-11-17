using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Manager;
using Stats;
using System.IO;

public class EnemySpawner : MonoBehaviour
{
    GameManager gameManager;
    LevelDifficultlyManager levelDifficultlyManager;

    [SerializeField] private Tilemap spawnTilemap; 
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private int spawnCountPerInterval = 4;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private int spawnValue = 0;
    private float totalSpawnWeight = 0f;

    [SerializeField] private List<EnemySpawnData> enemySpawnOptions = new List<EnemySpawnData>();

    private List<Vector3> spawnPositions = new List<Vector3>();

    [System.Serializable]
    public struct EnemySpawnData
    {
        public GameObject prefab;
        [Range(0f, 1f)] public float spawnChance;
        public int cost;
    }

    void Start()
    {
        gameManager = GameManager.Instance;
        levelDifficultlyManager = gameManager.levelStatManager.gameObject.GetComponent<LevelDifficultlyManager>();
        spawnValue = levelDifficultlyManager.enemySpawnDifficulty;

        CollectSpawnPositions();
        //SetSpawnIntervalBaseOnDifficulty();

        foreach (var option in enemySpawnOptions)
        {
            totalSpawnWeight += option.spawnChance;
        } if (totalSpawnWeight <= 0f) Debug.LogWarning("No valid spawn chance, enemy won't spawn.");

        InvokeRepeating(nameof(SpawnEnemy), 0f, spawnInterval);
    }

    private void CollectSpawnPositions()
    {
        BoundsInt bounds = spawnTilemap.cellBounds;

        // Set bound first then check each cell if it has a tile there
        foreach (Vector3Int cellPos in bounds.allPositionsWithin)
        {
            if (spawnTilemap.HasTile(cellPos))
            {
                Vector3 worldPos = spawnTilemap.GetCellCenterWorld(cellPos);
                spawnPositions.Add(worldPos);
            }
        }
        Debug.Log($"Found {spawnPositions.Count} spawn position counts!");
    }

    private void SpawnEnemy()
    {
        if (spawnPositions.Count == 0 || spawnValue <= 0 || enemySpawnOptions.Count == 0) return;

        int attemptsPerInterval = spawnCountPerInterval; 
        while (attemptsPerInterval > 0 && spawnValue > 0)
        {
            float randomWeight = Random.Range(0f, totalSpawnWeight);
            EnemySpawnData selected = new EnemySpawnData();
            float cumulative = 0f;

            foreach (var option in enemySpawnOptions)
            {
                cumulative += option.spawnChance;
                if (randomWeight <= cumulative)
                {
                    selected = option; break;
                }
            }

            if (selected.cost > spawnValue || selected.prefab == null)
            {
                enemySpawnOptions.Remove(selected);
                attemptsPerInterval--;
                continue;
            }

            int randomIndex = Random.Range(0, spawnPositions.Count);
            Vector3 spawnPos = spawnPositions[randomIndex];

            GameObject enemy = Instantiate(selected.prefab, spawnPos, Quaternion.identity);
            spawnValue -= selected.cost;

            attemptsPerInterval--;
        }
    }

    private void SetSpawnIntervalBaseOnDifficulty()
    {
        switch (gameManager.curDifficulty)
        {
            case Difficulty.Normal:
                spawnInterval = 4f;
                break;

            case Difficulty.Hard:
                spawnInterval = 2f;
                break;

            case Difficulty.Nightmare:
                spawnInterval = 1f;
                break;
        }
    }
    public void StartSpawn()
    {
        spawnValue = levelDifficultlyManager.enemySpawnDifficulty;

        InvokeRepeating(nameof(SpawnEnemy), 0, spawnInterval);
    }
}
