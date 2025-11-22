using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stats;
using Ai;
using Manager;

public class BossSpawner : EnemySpawner
{
    [Header("Boss Spawner")]
    [SerializeField] GameObject bossGO;

    protected override void Start()
    {
        
    }

    public void ActivateBoss(RoomComponent roomComponent)
    {
        partOfRoomComponent = roomComponent;

        AiAgent agent = bossGO.GetComponent<AiAgent>();
        AiAttack aiAttack = bossGO.GetComponent<AiAttack>();
        AiStat aiStat = bossGO.GetComponent<AiStat>();

        if (bossGO.gameObject.activeSelf == false) bossGO.gameObject.SetActive(true);
        agent.SetNewTarget(GameManager.Instance.player.transform);
        aiAttack.target = GameManager.Instance.player.transform;
        aiStat.partOfRoomComponent.activeEnemy.Add(bossGO.gameObject);
    }

    void StartSpawnBoss()
    {

    }
}
