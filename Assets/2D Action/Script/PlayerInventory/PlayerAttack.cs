using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stats;


public class PlayerAttack : MonoBehaviour
{
    public List<GameObject[]> playerWeaponInventory;

    public GameObject leftHandWeapon;
    public GameObject rightHandWeapon;

    private PlayerStat playerStat;

    // Start is called before the first frame update
    void Start()
    {
        playerStat = GetComponent<PlayerStat>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartAttack(bool isLeftWeapon)
    {
        if (isLeftWeapon && leftHandWeapon != null)
        {
            if (leftHandWeapon.GetComponent<AttackStat>().attackType == AttackStat.AttackType.melee)
            {
                MeleeAttack meleeAttack = leftHandWeapon.GetComponent<MeleeAttack>();
                StartCoroutine(meleeAttack.AttackCoroutine());
                playerStat.TriggerShake(5);
            }


        } else if (!isLeftWeapon)
        {
            Instantiate(rightHandWeapon, transform.position, gameObject.transform.rotation);

            /*if (rightHandWeapon.GetComponent<AttackStat>().attackType == AttackStat.AttackType.range)
            {
                ProjectileAttack rangeAttack = rightHandWeapon.GetComponent<ProjectileAttack>();
                Instantiate(rightHandWeapon, transform.position, gameObject.transform.rotation);
            }*/
        }
    }


}
