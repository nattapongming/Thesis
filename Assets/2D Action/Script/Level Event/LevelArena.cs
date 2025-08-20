using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelArena : MonoBehaviour
{
    public int activespawnpoint = 0;
    [SerializeField] List<EnemySpawn> enemySpawnPoint;
    [SerializeField] GameObject arenaBarrier;
    bool isActive;

    int currenentwave;
    int maxwave;


    // Start is called before the first frame update
    void Start()
    {
        foreach (EnemySpawn enemySpawn in enemySpawnPoint)
        {
            enemySpawn.levelArena = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (activespawnpoint <= 0 && isActive)
        {
            FinishArena();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isActive)
        {
            isActive = true;
            activespawnpoint = enemySpawnPoint.Count;
            Debug.Log("Enter Arena");
            foreach (EnemySpawn spawnpoint in enemySpawnPoint)
            {
                spawnpoint.isActive = true;
            }
            arenaBarrier.SetActive(true);
            
        }

       
    }

    private void FinishArena()
    {
        Debug.Log("Finish Arena");
        Destroy(arenaBarrier);
        Destroy(gameObject);
        GameManager.Instance.levelStatManager.arenaComplete++;
    }


}
