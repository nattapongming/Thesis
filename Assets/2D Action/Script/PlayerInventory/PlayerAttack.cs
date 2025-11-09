using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stats;
using UnityEngine.UI;
using System;
using UnityEngine.Sprites;

public class PlayerAttack : MonoBehaviour
{
    public List<GameObject> playerWeaponInventory;
    public List<GameObject> runtimeWeaponInstances = new List<GameObject>();
    public GameObject curWeaponInstance;
    private AttackStat curWeaponAttackStat;
    [SerializeField] private GameObject weaponSpawnPoint;
    [SerializeField] private Image weaponSprite;
    [SerializeField] private SpriteRenderer weaponGOSprite;

    private PlayerStat playerStat;
    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerStat = GetComponent<PlayerStat>();
        playerController = GetComponent<PlayerController>();
        if (playerWeaponInventory.Count > 0)
        {
            UpdateWeaponSlot();
            SwitchToWeapon(0);
            SetWeaponSprite(runtimeWeaponInstances[0].GetComponent<AttackStat>().attackSprite);

        }
    }

    private void Update()
    {
        playerStat.playerCurrentAttackCoolDown += Time.deltaTime;
        if (playerController.isAttacking && playerStat.playerCurrentAttackCoolDown >= curWeaponAttackStat.attackCoolDown)
        {
            StartAttack();
        }
    }

    public void StartAttack()
    {
        if (curWeaponInstance == null) return;

        curWeaponAttackStat = curWeaponInstance.GetComponent<AttackStat>();
        if (playerStat.playerCurrentAttackCoolDown > curWeaponAttackStat.attackCoolDown)
        {
            playerStat.playerCurrentAttackCoolDown = 0;
        }
        else
        {
            return;
        }

        switch (curWeaponAttackStat.attackType)
        {
            case AttackStat.AttackType.melee:
                MeleeAttack meleeAttack = curWeaponAttackStat as MeleeAttack;
                StartCoroutine(meleeAttack.MeleeAttackCoroutine(playerStat));
                break;

            case AttackStat.AttackType.range:

                ProjectileAttack projectile = Instantiate(curWeaponInstance, weaponSpawnPoint.transform.position, weaponSpawnPoint.transform.rotation).GetComponent<ProjectileAttack>();
                if (!projectile.gameObject.activeSelf) projectile.gameObject.SetActive(true);
                StartCoroutine(projectile.ProjectileAttackCoroutine(playerStat));
                break;

            default:
                Debug.LogError("Error! This cur weapon doesn't have attack stat!");
                break;
        }


    }

    public void SetWeaponSprite(Sprite sprite)
    {
        weaponSprite.sprite = sprite;
    }

    public void UpdateWeaponSlot()
    {
        // Clear old runtime instance
        foreach (var instance in runtimeWeaponInstances)
        {
            if (instance != null) Destroy(instance);
        }
        runtimeWeaponInstances.Clear();

        // Create fresh instances from prefabs then apply enchantments
        for (int i = 0; i < playerWeaponInventory.Count; i++)
        {
            GameObject prefab = playerWeaponInventory[i];
            
            GameObject instance = Instantiate(prefab); // Create copy
            instance.SetActive(false); // Keep inactive until equipped
            instance.name = prefab.name + "_Runtime";
            AttackStat stat = instance.GetComponent<AttackStat>();
            curWeaponAttackStat = stat;
            if (stat != null)
            {

                stat.weaponIndex = i;
                stat.ResetToOriginalStats(); // Reset before apply
                //Debug.Log($"Call enchantment of {stat}");
                stat.CallPlayerWeaponEnchantment(); // Apply enchantments to instance
            }

            runtimeWeaponInstances.Add(instance);
        }

        // Refresh current if equipped
        if (curWeaponInstance != null)
        {
            int currentIndex = runtimeWeaponInstances.IndexOf(curWeaponInstance);
            if (currentIndex >= 0)
            {
                SwitchToWeapon(currentIndex); // Re-equip to new instance
            }
        }
    }

    private void SwitchToWeapon(int index)
    {
        if (index < 0 || index >= runtimeWeaponInstances.Count) return;

        // Deactivate old
        if (curWeaponInstance != null)
        {
            curWeaponInstance.SetActive(false);
            // Optional: Reparent or reposition if needed
        }

        // Activate new
        curWeaponInstance = runtimeWeaponInstances[index];
        if (curWeaponInstance.GetComponent<AttackStat>().attackType == AttackStat.AttackType.melee)
        {
            curWeaponInstance.transform.SetParent(weaponSpawnPoint.transform); // Parent to player if needed for positioning
        }
        curWeaponInstance.transform.localPosition = new Vector3(0.5f, 0, 0); // Adjust as needed
        curWeaponInstance.SetActive(true);

        AttackStat stat = curWeaponInstance.GetComponent<AttackStat>();
        SetWeaponSprite(stat.attackSprite);
    }

    // Edge case: Reset all on disable/quit
    void OnDisable()
    {
        foreach (var instance in runtimeWeaponInstances)
        {
            if (instance == null) continue;
            AttackStat stat = instance?.GetComponent<AttackStat>();
            stat?.ResetToOriginalStats();
        }
    }
}
