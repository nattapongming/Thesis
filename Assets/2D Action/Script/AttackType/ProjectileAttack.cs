using Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : AttackStat
{
    protected ProjectileMovement projectileMovement;

    protected override void Awake()
    {
        base.Awake();
        projectileMovement = GetComponent<ProjectileMovement>();

    }

    protected override void Start()
    {
        base.Start();

        sprite = GetComponent<SpriteRenderer>();
        if (affectByEnchant)
        CallPlayerWeaponEnchantment();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator ProjectileAttackCoroutine(StatInfo statInfo)
    {
        statInfo.isAttacking = true;
        yield return new WaitForSeconds(0.1f);

        statInfo.isAttacking = false;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        StatInfo otherStat = collision.gameObject.GetComponent<StatInfo>();
        if (!otherStat) return;
        //Debug.Log($"Other gameobject is {collision.gameObject.name} and faction is {otherStat.faction}, self faction is {faction}");
        
        if (projectileMovement.onHitDestory && otherStat.faction != faction && !otherStat.isInvincible) 
        {
            Destroy(gameObject);
        }

    }
}
