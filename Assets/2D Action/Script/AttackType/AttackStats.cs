using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;

namespace Stats
{
    public class AttackStat : MonoBehaviour
    {
        public enum AttackType { melee, range }


        [Header("General stats")]
        public float damage = 1;
        public float managain = 0.1f;
        public Faction faction;
        public AttackType attackType;
        public Sprite attackSprite;

        [Header("Enchantment stats")]
        public GameObject[] enchantment = new GameObject[5];
        public bool affectByEnchant = false;


        protected BoxCollider2D boxCollider;
        protected SpriteRenderer sprite;
        protected AttackEnchantmentApplier attackEnchantmentApplier;

        // Start is called before the first frame update

        protected void Awake()
        {
            if(affectByEnchant && enchantment.Length > 0)
            {
                attackEnchantmentApplier = new AttackEnchantmentApplier(this);
            }
        }

        protected virtual void Start()
        {
            boxCollider = GetComponent<BoxCollider2D>();
            sprite = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
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