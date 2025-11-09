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
        public SpriteRenderer spriteGO;
        public float damage = 1;
        public float managain = 0.1f;
        public float attackCoolDown = 0.2f;

        [Header("Original stats")]
        public float ogDamage = 0;
        public float ogManagain = 0;
        public float ogAttackCoolDown = 0;
        private Vector3 ogScale;

        // For colllider (in case of many type)
        private Vector2 ogBoxSize;
        private float ogCircleRadius;
        private Vector2 ogCapsuleSize;
        private Vector2[] ogPolygonPoints;

        [Header("Enchantment stats")]
        //public ScriptableObject[] enchantment = new ScriptableObject[5];
        public bool affectByEnchant = false;
        public int weaponIndex = -1;

        [SerializeField] protected BoxCollider2D boxCollider;
        [SerializeField] protected SpriteRenderer sprite;
        protected AttackEnchantmentApplier attackEnchantmentApplier;

        // Start is called before the first frame update

        protected virtual void Awake()
        {
            if (ogDamage == 0) ogDamage = damage;
            if (ogManagain == 0) ogManagain = managain;
            if (ogAttackCoolDown == 0) ogAttackCoolDown = attackCoolDown;

            ogScale = transform.localScale;

            if (TryGetComponent<BoxCollider2D>(out var box)) ogBoxSize = box.size;
            if (TryGetComponent<CircleCollider2D>(out var circle)) ogCircleRadius = circle.radius;
            if (TryGetComponent<CapsuleCollider2D>(out var capsule)) ogCapsuleSize = capsule.size;
            if (TryGetComponent<PolygonCollider2D>(out var polygon)) ogPolygonPoints = polygon.points;
        }

        protected virtual void Start()
        {
            boxCollider = GetComponent<BoxCollider2D>();
            sprite = GetComponent<SpriteRenderer>();
        }

        
        public void ResetToOriginalStats()
        {
            damage = ogDamage;
            managain = ogManagain;
            attackCoolDown = ogAttackCoolDown;

            transform.localScale = ogScale;

            ResetColliderSize();
        }

        private void ResetColliderSize()
        {
            if (TryGetComponent<BoxCollider2D>(out var box))
            {
                box.size = ogBoxSize; // Assign, not multiply
            }
            else if (TryGetComponent<CircleCollider2D>(out var circle))
            {
                circle.radius = ogCircleRadius;
            }
            else if (TryGetComponent<CapsuleCollider2D>(out var capsule))
            {
                capsule.size = ogCapsuleSize;
            }
            else if (TryGetComponent<PolygonCollider2D>(out var polygon))
            {
                polygon.points = (Vector2[])ogPolygonPoints.Clone(); // Clone to avoid ref issues
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            //Debug.Log($"Self {gameObject.name} other {collision.gameObject.name}");

            if (collision.CompareTag("Enemy") || collision.CompareTag("Player"))
            {
                StatInfo otherStat = collision.gameObject.GetComponent<StatInfo>();
                if (otherStat.faction != faction) otherStat.TakeDamage(damage);

            }
            else return;

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
            ResetToOriginalStats();
            attackEnchantmentApplier = new AttackEnchantmentApplier(this);
        }

        private void OnDisable()
        {
            ResetToOriginalStats();
        }
    }
}