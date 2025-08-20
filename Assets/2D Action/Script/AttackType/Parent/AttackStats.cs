using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    public class AttackStat : MonoBehaviour
    {
        public enum AttackType { melee, range }


        [Header("General stats")]
        public Faction faction;
        public AttackType attackType;
        public Sprite attackSprite;
        public float damage = 1;
        public float managain = 0.1f;
        public float attackCoolDown = 0.2f;

        [Header("Enchantment stats")]
        public ScriptableObject[] enchantment = new ScriptableObject[5];
        public bool affectByEnchant = false;
        public int weaponIndex = -1;

        [SerializeField] protected BoxCollider2D boxCollider;
        [SerializeField] protected SpriteRenderer sprite;
        protected AttackEnchantmentApplier attackEnchantmentApplier;

        // Start is called before the first frame update

        protected void Awake()
        {
            /*if(affectByEnchant && enchantment.Length > 0 && weaponIndex != -1)
            {
                Debug.Log("working!");
                attackEnchantmentApplier = new AttackEnchantmentApplier(this);
                //Debug.Log($"Create attackenchantapplier of {this.gameObject}");
            }*/
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

        public void CallPlayerWeaponEnchantment()
        {
            attackEnchantmentApplier = new AttackEnchantmentApplier(this);
        }
    }
}