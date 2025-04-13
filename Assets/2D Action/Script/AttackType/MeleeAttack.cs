using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    public class MeleeAttack : AttackStat
    {
        [SerializeField] float attackDuration = 0.1f;
        [SerializeField] float attackCoolDown = 0.2f;


        // Update is called once per frame
        void Update()
        {
            
        }

        public IEnumerator MeleeAttackCoroutine(StatInfo statInfo)
        {
            ActiveAttack();
            statInfo.isAttacking = true;

            //Debug.Log($"Is attacking = {statInfo.isAttacking}");
            yield return new WaitForSeconds(attackDuration);

            statInfo.isAttacking = false;
            //Debug.Log($"Is attacking = {statInfo.isAttacking}");

            DeactiveAttack();
        }
    }
}