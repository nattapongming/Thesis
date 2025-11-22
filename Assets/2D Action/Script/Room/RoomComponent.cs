using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomComponent : MonoBehaviour
{
    public enum RoomType { Area, Boss }

    [Header("Room Metadata")]
    public float roomWidth = 20f; // In world units (e.g., number of tiles * tile size)
    [SerializeField] bool isRoomStart;
    public List<GameObject> activeEnemy;

    [Header("Room Type")]
    [SerializeField] RoomType roomType = RoomType.Area;

    [Header("Room Component")]
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] GameObject wallTileMapGO;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateRoom()
    {
        if (activeEnemy.Count <= 0 && isRoomStart)
        {
            Debug.Log("Finish Room");
            wallTileMapGO.SetActive(false);
        }
    }

    public void StartRoom()
    {
        Debug.Log("Start Room");

        switch (roomType)
        {
            case RoomType.Area:
                enemySpawner.StartSpawn(this);
                 
                break;

            case RoomType.Boss:
                BossSpawner bossSpawner = enemySpawner as BossSpawner;
                bossSpawner.ActivateBoss(this);
                break;

            default: return;
        }

        wallTileMapGO.SetActive(true);
    }

    
}
