using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Stats
{
    public class MeleeAttack : AttackStat
    {
        [SerializeField] float attackDuration = 0.1f;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            FitColliderToSprite(boxCollider);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public IEnumerator MeleeAttackCoroutine(StatInfo statInfo)
        {
            ActiveAttack();
            statInfo.isAttacking = true;

            yield return new WaitForSeconds(attackDuration);

            statInfo.isAttacking = false;
            DeactiveAttack();
        }

        void FitColliderToSprite(BoxCollider2D box)
        {
            SpriteRenderer sr = box.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite != null)
            {
                box.size = sr.sprite.bounds.size;
                box.offset = sr.sprite.bounds.center;
            }
        }

    }
}