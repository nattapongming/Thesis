using Animation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AiAttack_Boss1 : AiAttack_Boss
{
    [Header("Lunge Attack")]
    [SerializeField] float lungeAttackcurveHeightMultiplier = 0.1f;
    [SerializeField] float bossLungeDistance = 6.5f;
    [SerializeField] float bossLungeSpeed = 8f;
    [SerializeField] bool lungeFaceAlongCurve = false;
    [SerializeField] GameObject afterLungeAttackPatternGO;

    [Header("Sword swing attack")]
    [SerializeField] float swordSwingDuration = 0.5f;
    [SerializeField] GameObject swordSwingAttackPatternGO;

    [Header("Sword smash attack")]
    [SerializeField] float swordSmashDuration = 0.5f;
    [SerializeField] GameObject swordSmashAttackPatternGO;

    [Header("Ground pound attack")]
    [SerializeField] float groundPoundAttackcurveHeightMultiplier = 0.8f;
    [SerializeField] float groundPoundDistance = 9f;
    [SerializeField] float groundPoundSpeed = 8f;
    [SerializeField] bool groundPoundFaceAlongCurve = false;

    [SerializeField] int afterGroundPoundAttackPatternAmount = 8;
    [SerializeField] float groundPoundMinRayDist = 2f;
    [SerializeField] float groundPoundMaxRayDist = 8f;
    [SerializeField] GameObject afterGroundPoundAttackPatternGO;

    [Header("Ulitmate attack")]
    [SerializeField] float ultimateAttackDuration = 5f;
    [SerializeField] float ultimateSpamInterval = 0.1f;
    [SerializeField] float ultimateRotateSpeed = 180f;
    [SerializeField] GameObject ultimateAttackPatternGO;

    [Header("Misc")]
    [SerializeField] GameObject jumpingVFX;
    [SerializeField] Sprite attackSprite1;
    [SerializeField] Sprite attackSprite2;

    Rigidbody2D rb;

    protected override void Start()
    {
        base.Start();
        spriteRenderer.flipX = true;
        currentAttackPattern = GetRandomAttackPattern();

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        aiAnimation = GetComponent<AiAnimation>();
    }

    protected override void Update()
    {
        float horizontalVel = rb.velocity.x;
        if (Mathf.Abs(horizontalVel) > 0.1f)
        {
            bool facingRight = horizontalVel > 0;
            spriteRenderer.flipX = !facingRight;  
        }

        if (!aiStat.isAttacking)
        attackCurrentCooldown += Time.deltaTime;
        if (attackCurrentCooldown >= GetEffectiveAttackCooldown() && CheckTargetRange() && !aiStat.isAttacking)
        {
            attackCurrentCooldown = 0;
            BossAttackPattern();
            currentAttackPattern = GetRandomAttackPattern();
        }
    }

    void BossAttackPattern()
    {
        aiStat.isAttacking = true;
        switch (currentAttackPattern)
        {
            case 0: LungeAttack(); break;
            case 1: SwordSwingAttack(); break;
            case 2: SwordSmashAttack(); break;
            case 3: GroundPoundAttack(); break;
            case 4: UltimateAttack(); break;
        }
    }


    void LungeAttack() => StartCoroutine(LungeCoroutine());
    void GroundPoundAttack() => StartCoroutine(GroundPoundCoroutine());
    void SwordSwingAttack() => StartCoroutine(SimpleAttackCoroutine(swordSwingAttackPatternGO, swordSwingDuration, false));
    void SwordSmashAttack() => StartCoroutine(SimpleAttackCoroutine(swordSmashAttackPatternGO, swordSmashDuration, false)); 
    void UltimateAttack() => StartCoroutine(UltimateAttackCoroutine());
    
    IEnumerator LungeCoroutine()
    {
        Vector2 lungeDir = (target.transform.position - transform.position).normalized;
        yield return StartCoroutine(CurvedAttackCoroutine(bossLungeSpeed, bossLungeDistance, lungeAttackcurveHeightMultiplier, lungeFaceAlongCurve));
        int phase = GetCurrentPhase();
        if (phase >= 1 && afterLungeAttackPatternGO != null && agent.target != null)
        {
            ShootPattern(afterLungeAttackPatternGO, lungeDir);
            Debug.Log("Follow up attack!");
        }
        aiStat.isAttacking = false;
    }
    IEnumerator GroundPoundCoroutine()
    {
        yield return StartCoroutine(CurvedAttackCoroutine(groundPoundSpeed, groundPoundDistance, groundPoundAttackcurveHeightMultiplier, groundPoundFaceAlongCurve));

        int actualAttackBullet = (afterGroundPoundAttackPatternAmount * GetCurrentPhase());
        Debug.Log($"Shoot {afterGroundPoundAttackPatternAmount} curve attack!");
        for (int i = 0; i < afterGroundPoundAttackPatternAmount; i++)
        {
            // Random ray for dir/dist scout
            float randAngle = Random.Range(0f, 360f);
            Vector2 randDir = new Vector2(Mathf.Cos(randAngle * Mathf.Deg2Rad), Mathf.Sin(randAngle * Mathf.Deg2Rad));
            float randRayDist = Random.Range(groundPoundMinRayDist, groundPoundMaxRayDist);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, randDir, randRayDist, wallLayerMask);

            Transform curveTarget = null;
            if (hit.collider != null)
            {
                int hitLayer = hit.collider.gameObject.layer;
                bool isWallLayer = (wallLayerMask.value & (1 << hitLayer)) != 0;
                bool isWallTag = hit.collider.CompareTag("Wall");
                if (isWallLayer && isWallTag)
                {
                    curveTarget = hit.collider.transform; // Hit wall? Curve TO it!
                    //Debug.DrawRay(transform.position, randDir * hit.distance, Color.cyan, 2f);
                    //Debug.Log($"Curve to wall: {hit.collider.name}");
                }
            }

            if (curveTarget == null)
            {
                // No wall? Dummy at ray end—always shoot!
                GameObject dummy = new GameObject("CurveDummy");
                dummy.transform.position = transform.position + (Vector3)(randDir * randRayDist);
                dummy.transform.parent = transform; // Optional tidy
                curveTarget = dummy.transform;
                Destroy(dummy, 0.1f); // Poof after snapshot!
            }

            // ALWAYS shoot curve to target!
            ShootCurveAttack(afterGroundPoundAttackPatternGO, curveTarget);
        }

        yield return new WaitForSeconds(0.75f);
        aiStat.isAttacking = false;
    }
    IEnumerator CurvedAttackCoroutine(float speed, float maxDist, float curveMult, bool faceAlongCurve)
    {

        aiStat.UpdateSpeed(0);
        yield return StartCoroutine(ChangeSpriteColor(0.25f));
        aiStat.UpdateSpeed(aiStat.speed);

        aiAnimation.SetTrigger("JumpUp");
        bool jumpDownTriggered = false;

        GameObject vfx = Instantiate(jumpingVFX);
        vfx.transform.position = transform.position;

        Transform targetTransform = agent.target;
        if (targetTransform == null) { aiStat.isAttacking = false; yield return null; }
        
        Vector2 startPos = transform.position;
        Vector2 targetPos = targetTransform.position;
        Vector2 flightDir = (targetPos - startPos).normalized;
        
        // Curve Distance
        float actualDist = Mathf.Min(Vector2.Distance(startPos, targetPos), maxDist);
        Vector2 endPos = startPos + flightDir * actualDist;

        Vector2 curvePerp = Vector2.up;
        float maxCurveHeight = actualDist * curveMult;
        float progress = 0f;
        Vector2 lastPos = startPos;


        if (faceAlongCurve)
        {
            float initAngle = Mathf.Atan2(flightDir.y, flightDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, initAngle);
        }

        
        while (progress < 1f)
        {
            if (progress > 0.5f && !jumpDownTriggered)
            {
                aiAnimation.SetParameter("IsJumpDown", true);
                jumpDownTriggered = true;
            }

            progress += (speed * Time.deltaTime) / actualDist;
            progress = Mathf.Clamp01(progress);

            Vector2 straightPos = Vector2.Lerp(startPos, endPos, progress);
            float curveOffset = Mathf.Sin(progress * Mathf.PI) * maxCurveHeight;
            Vector2 curvyPos = straightPos + (curvePerp * curveOffset);
            transform.position = curvyPos;

            if (faceAlongCurve)
            {
                Vector2 vel = (curvyPos - lastPos) / Time.deltaTime;
                if (vel != Vector2.zero)
                {
                    float angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, 0f, angle);
                }
            }
            lastPos = curvyPos;
            yield return null;
        }

        transform.position = endPos;

        aiAnimation.SetParameter("IsJumpDown", false);
        jumpDownTriggered = false;
        
    }
    IEnumerator SimpleAttackCoroutine(GameObject patternGO, float duration, bool rotateBulletByDir)
    {
        if (patternGO == null || target == null)
        {
            aiStat.isAttacking = false;
            yield break;
        }

        aiStat.UpdateSpeed(0f);

        yield return StartCoroutine(ChangeSpriteColor(0.25f));
        //yield return new WaitForSeconds(duration);

        aiStat.UpdateSpeed(aiStat.speed);
        Vector2 dirToTarget = ((Vector2)target.transform.position -  (Vector2)transform.position).normalized;
        ShootPattern(patternGO, dirToTarget);
        aiStat.isAttacking = false;
    }
    IEnumerator UltimateAttackCoroutine()
    {
        if (ultimateAttackPatternGO == null) { aiStat.isAttacking = false; yield break; }
        aiStat.UpdateSpeed(0);

        float angle = Random.Range(0f, 360f);
        bool rotateCW = Random.value > 0.5f;
        float changeTime = Random.Range(0.5f, 2.5f);
        float switchChance = Random.Range(25f, 75f) / 100f; // % as 0-1
        float elapsed = 0f;

        while (elapsed < ultimateAttackDuration)
        {
            // Rotate 
            angle += (rotateCW ? ultimateRotateSpeed : -ultimateRotateSpeed) * ultimateSpamInterval;

            if (elapsed > changeTime && Random.value < switchChance)
            {
                rotateCW = !rotateCW;
                changeTime = elapsed + Random.Range(0.5f, 2.5f);
                switchChance = Random.Range(25f, 75f) / 100f;
            }

            // 3x 120 degree brust
            for (int i = 0; i < 3; i++)
            {
                float burstAng = angle + (i * 120f);
                Vector2 burstDir = new Vector2(Mathf.Cos(burstAng * Mathf.Deg2Rad), Mathf.Sin(burstAng * Mathf.Deg2Rad));
                ShootAttack(ultimateAttackPatternGO, burstDir);
            }

            yield return new WaitForSeconds(ultimateSpamInterval);
            elapsed += ultimateSpamInterval;
        }

        StartCoroutine(LungeCoroutine());
        aiStat.isAttacking = false;
        aiStat.UpdateSpeed(aiStat.speed);
    }


    
}
