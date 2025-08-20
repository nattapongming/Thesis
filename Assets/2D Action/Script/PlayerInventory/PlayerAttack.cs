using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stats;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    public List<GameObject> playerWeaponInventory;
    public GameObject curWeapon;
    [SerializeField] private GameObject weaponSpawnPoint;

    private PlayerStat playerStat;
    [SerializeField] private Image weaponSprite;
    // Start is called before the first frame update
    void Start()
    {        
        playerStat = GetComponent<PlayerStat>();
        if (playerWeaponInventory.Count > 0)
        {
            curWeapon = playerWeaponInventory[0];
            SetWeaponSprite(playerWeaponInventory[0].GetComponent<AttackStat>().attackSprite);
            UpdateWeaponSlot();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartAttack()
    {
        AttackStat curWeaponStat = curWeapon.GetComponent<AttackStat>();

        switch (curWeaponStat.attackType)
        {
            case AttackStat.AttackType.melee:
                MeleeAttack meleeAttack = curWeaponStat as MeleeAttack;
                StartCoroutine(meleeAttack.MeleeAttackCoroutine(playerStat));
                break;

            case AttackStat.AttackType.range:
                //ProjectileMovement projectileMovement = Instantiate(curWeapon, transform.position, Quaternion.identity).GetComponent<ProjectileMovement>();
                //meObject projectile = Instantiate(curWeapon, weaponSpawnPoint.transform.position, weaponSpawnPoint.transform.rotation);

                ProjectileAttack projectile = Instantiate(curWeapon, weaponSpawnPoint.transform.position, weaponSpawnPoint.transform.rotation).GetComponent<ProjectileAttack>();
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
    { int i = 0;
        foreach (GameObject attack in playerWeaponInventory)
        {
            attack.GetComponent<AttackStat>().weaponIndex = i;
            attack.GetComponent<AttackStat>().CallPlayerWeaponEnchantment();
            i++;
            //Debug.Log($"This {attack} index is {attack.GetComponent<AttackStat>().weaponIndex}");
        }
    }
}
