using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ai;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] GameObject EnemyPrefab;

    [SerializeField] Transform target;

    [SerializeField] int enemyToSPawn = 5;
    [SerializeField] float spawnCoolDown = 1.5f;
    [SerializeField] float spawnTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyToSPawn > 0 )
        {
            if (spawnTimer < spawnCoolDown)
            {
                spawnTimer += Time.deltaTime;
            } else
            {
                spawnTimer = 0;
                GameObject enemy = Instantiate(EnemyPrefab, transform.position, Quaternion.identity);
                enemy.GetComponent<AiAgent>().target = target;
            }

        }
    }
}
