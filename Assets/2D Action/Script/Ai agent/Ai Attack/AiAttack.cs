using Ai;
using Animation;
using Bullet;
using Manager;
using Movement;
using Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAttack : MonoBehaviour
{
    protected enum AttackPatternType { Shoot, Lunge }

    [Header("Info")]
    public Transform target;
    [SerializeField] float range = 1f;
    [SerializeField] GameObject turret;

    [Header("Other component")]
    [SerializeField] protected AiAnimation aiAnimation;
    [SerializeField] protected SpriteRenderer spriteRenderer;

    [Header("AttackSpeed")]
    [SerializeField] protected float baseAttackCooldown = 2f;
    [SerializeField] protected float minAttackCooldown = 0.1f;
    [SerializeField] protected float currentAttackCooldown;
    [SerializeField] protected float attackCurrentCooldown = 0f;

    [SerializeField] protected float currentAttackDelay = 0f;

    [Header("Attack pattern")]
    //[SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected GameObject[] attackPattern;
    [SerializeField] protected float[] attackDelay;
    [SerializeField] protected AttackPatternType[] attackPatternTypes = new AttackPatternType[] { AttackPatternType.Shoot };

    [Header("Targeting")]
    [SerializeField] protected LayerMask wallLayerMask = 1 << 8;  // Layer 8 only - set in Inspector if needed!

    [Header("Lunge Attack")]
    [SerializeField] protected float lungeDistance = 4f;
    [SerializeField] protected float lungeSpeed = 25f;
    [SerializeField] protected float lungeDelay = 0.5f;



    protected GameManager gameManager;
    protected AiStat aiStat;
    [SerializeField] protected AiAgent agent;
    protected Rigidbody2D rb;

    protected Vector3 direction;


    // Start is called before the first frame update
    virtual protected void Start()
    {
        gameManager = GameManager.Instance;

        if (!rb) rb = GetComponent<Rigidbody2D>();
        if (!agent) agent = GetComponent<AiAgent>();
        if (!aiStat) aiStat = GetComponent<AiStat>();
        if (!aiAnimation) aiAnimation = GetComponent<AiAnimation>();
        if (!spriteRenderer) spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        if (!aiStat.isAttacking)
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
            Debug.DrawRay(transform.position, direction * range, Color.red, 0.5f);  // Only draw when outta range!
            return false;
        }

        //Debug.Log("target in range");
        return true;
    }

    protected bool CheckForTarget()
    {
        Vector2 toTarget = (Vector2)(target.position - transform.position);
        float dist = toTarget.magnitude;
        Vector2 direction = toTarget.normalized;

        RaycastHit2D wallHit = Physics2D.Raycast(transform.position, direction, dist, wallLayerMask);

        if (wallHit.collider != null)
        {
            int hitLayer = wallHit.collider.gameObject.layer;
            //Debug.Log($"Wall hit is {wallHit.collider.gameObject.name} and layer is {hitLayer} (mask value: {wallLayerMask.value})");

            bool isWallLayer = (wallLayerMask.value & (1 << hitLayer)) != 0;
            bool isWallTag = wallHit.collider.CompareTag("Wall");

            if (isWallLayer && isWallTag)
            {
                //Debug.DrawRay(transform.position, direction * dist, Color.red, 0.5f);
                return false;
            }
        }

        return true;
        //ShootAttack(attackPattern[0], direction);
        //Debug.DrawRay(transform.position, direction * dist, Color.green, 0.5f);
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

    protected void LungeAttack(Vector2 dir, float distance, float lungeSpeed)
    {
        dir = dir.normalized;
        StartCoroutine(LungeAttackCoroutine(dir, distance, lungeSpeed, lungeDelay));
    }

    protected IEnumerator LungeAttackCoroutine(Vector2 directionIn, float distance, float lungeSpeed, float lungeDelay)
    {
        aiStat.isAttacking = true;
        Transform cachedTarget = agent.target;
        agent.SetNewTarget(null);
        aiStat.UpdateSpeed(0);

        aiAnimation.SetParameter("IsAttack", true);

        yield return new WaitForSeconds(lungeDelay);

        aiAnimation.SetParameter("IsAttack", false);

        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + (Vector3)(directionIn.normalized * distance);

        float timer = 0f;
        float duration = distance / lungeSpeed;

        while (timer < duration)
        {
            if (enabled == false) break;
            timer += Time.deltaTime;
            float t = timer / duration;
            rb.MovePosition(Vector3.Lerp(startPos, targetPos, t));

            yield return null;
        }

        rb.MovePosition(targetPos);

        //agent.enabled = true;
        agent.nevMeshAgent.nextPosition = transform.position;
        agent.SetNewTarget(cachedTarget);
        aiStat.UpdateSpeed(aiStat.speed);
        aiStat.isAttacking = false;
    }

    // Misc Method
    protected Vector2 GetDirToTarget(Transform target)
    {
        target ??= this.target;
        if (target == null) return Vector2.zero;

        Vector2 dir = (target.position - transform.position);
        return dir.normalized;
    }
    
    
}
