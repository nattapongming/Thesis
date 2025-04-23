using Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : AttackStat
{
    protected ProjectileMovement projectileMovement;

    // Start is called before the first frame update
    protected override void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        projectileMovement = GetComponent<ProjectileMovement>();
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
        
        if (projectileMovement.onHitDestory) Destroy(gameObject);

    }
}
