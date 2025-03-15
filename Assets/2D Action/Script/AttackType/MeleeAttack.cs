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

        public IEnumerator AttackCoroutine()
        {
            ActiveAttack();

            yield return new WaitForSeconds(attackDuration);

            DeactiveAttack();
        }
    }
}