using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    public class ProjectileAttack : AttackStat
    {
        
        // Start is called before the first frame update
        protected override void Start()
        {
            sprite = GetComponent<SpriteRenderer>();
        }

        

        // Update is called once per frame
        void Update()
        {
            
            
        }

        public IEnumerator ProjectileAttackCoroutine(StatInfo statInfo)
        {
            statInfo.isAttacking = true;
            Debug.Log($"Is attacking = {statInfo.isAttacking}");
            yield return new WaitForSeconds(0.1f);

            statInfo.isAttacking = false;
            Debug.Log($"Is attacking = {statInfo.isAttacking}");
        }
    }
}