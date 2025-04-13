using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    public class AttackStat : MonoBehaviour
    {
        public enum AttackType { melee, range}
        
        public float damage = 1;
        public Faction faction;
        public AttackType attackType;

        protected BoxCollider2D boxCollider;
        protected SpriteRenderer sprite;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            boxCollider = GetComponent<BoxCollider2D>();
            sprite = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            //Debug.Log($"Self {gameObject.name} other {collision.gameObject.name}");
            
            if (collision.CompareTag("Enemy"))
            {

                StatInfo otherStat = collision.gameObject.GetComponent<StatInfo>();
                if (otherStat.faction != faction) otherStat.TakeDamage(damage);

            }

            
        }

        public void ActiveAttack()
        {

            boxCollider.enabled = true;
            sprite.enabled = true;
        }

        public void DeactiveAttack()
        {
            boxCollider.enabled = false;
            sprite.enabled = false;
        }

        
    }
}