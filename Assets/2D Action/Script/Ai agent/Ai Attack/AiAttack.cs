using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ai;
using Bullet;
using Stats;
using Movement;

public class AiAttack : MonoBehaviour
{
    [Header("Info")]
    public Transform target;
    [SerializeField] float range = 1f;
    [SerializeField] GameObject turret;

    [Header("AttackSpeed")]
    [SerializeField] protected float baseAttackCooldown = 2f;
    [SerializeField] protected float minAttackCooldown = 0.1f;
    [SerializeField] protected float currentAttackCooldown;
    [SerializeField] protected float attackCurrentCooldown = 0f;

    [SerializeField] protected float attackDelay = 0.5f;
    [SerializeField] protected float currentAttackDelay = 0f;

    [Header("Attack pattern")]
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected GameObject attackPattern;

    [Header("Targeting")]
    [SerializeField] protected LayerMask wallLayerMask = 1 << 8;  // Layer 8 only - set in Inspector if needed!

    protected GameManager gameManager;
    protected AiStat  aiStat;
    [SerializeField] protected AiAgent agent;

    protected Vector3 direction;


    // Start is called before the first frame update
    virtual protected void Start()
    {
        gameManager = GameManager.Instance;

        if (!agent) agent = GetComponent<AiAgent>();
        if (!aiStat) aiStat = GetComponent <AiStat>();
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        attackCurrentCooldown += Time.deltaTime;

        if (attackCurrentCooldown >= GetEffectiveAttackCooldown() && CheckTargetRange())
        {
            attackCurrentCooldown = 0f; 
            CheckForTarget();
        }

    }

    protected float GetEffectiveAttackCooldown()
    {
        float effective = baseAttackCooldown;
        // Get attackModifier in component
        AiAtkSpeedModifier[] modifiers = GetComponents<AiAtkSpeedModifier>();
        //if (modifiers.Length > 0) Debug.Log("Have ")
        foreach (AiAtkSpeedModifier mod in modifiers)
        {
            effective -= mod.GetCooldownReduction();  
        }
        currentAttackCooldown = Mathf.Max(minAttackCooldown, effective);
        return Mathf.Max(minAttackCooldown, effective);
    }

    protected bool CheckTargetRange()
    {
        if (target == null) return false;

        Vector2 toTarget = (Vector2)(target.position - transform.position);
        float dist = toTarget.magnitude;
        Vector2 direction = toTarget.normalized;

        Debug.DrawRay(transform.position, direction * range, Color.yellow, 0.5f);

        if (dist > range)
        {
            Debug.DrawRay(transform.position, direction * range, Color.yellow, 0.5f);  // Only draw when outta range!
            return false; 
        }

        return true;
    }

    protected void CheckForTarget()
    {
        Vector2 toTarget = (Vector2)(target.position - transform.position);
        float dist = toTarget.magnitude;
        Vector2 direction = toTarget.normalized;

        RaycastHit2D wallHit = Physics2D.Raycast(transform.position, direction, dist, wallLayerMask);

        if (wallHit.collider != null)
        {
            int hitLayer = wallHit.collider.gameObject.layer;
            Debug.Log($"Wall hit is {wallHit.collider.gameObject.name} and layer is {hitLayer} (mask value: {wallLayerMask.value})");

            bool isWallLayer = (wallLayerMask.value & (1 << hitLayer)) != 0;
            bool isWallTag = wallHit.collider.CompareTag("Wall");
            
            if (isWallLayer && isWallTag) 
            {
                Debug.DrawRay(transform.position, direction * dist, Color.red, 0.5f);
                return; 
            }
        }

        ShootAttack(attackPattern, direction);
        Debug.DrawRay(transform.position, direction * dist, Color.green, 0.5f);
    }

    protected void ShootAttack(GameObject prefab, Vector2 direction)
    {
        if (prefab == null) return;

        GameObject bullet = Instantiate(prefab);
        bullet.transform.position = transform.position;
        ProjectileMovement pm = bullet.GetComponent<ProjectileMovement>();
        if (pm != null)
        {
            pm.SetDirection(direction);  // Use normalized
        }
    }

    protected void ShootCurveAttack(GameObject prefab, Transform target)
    {
        if (prefab == null) return;

        GameObject bullet = Instantiate(prefab);
        bullet.transform.position = transform.position;
        CurveProjectileMovement cpm = bullet.GetComponent<CurveProjectileMovement>();
        if (cpm != null)
        {
            cpm.target = target;  // Use normalized
        }
    }

    protected void ShootPattern(GameObject prefab, Vector2 dir)
    {
        if (prefab == null) return;
        
        GameObject patternGO = Instantiate(prefab, transform);  // Child ok
        BulletPattern bulletPattern = patternGO.GetComponent<BulletPattern>();
        if (bulletPattern != null)
        {
            bulletPattern.direction = dir;  // Normalized for clean spread!
        }
    }

}
