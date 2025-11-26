using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AiAttack_Enemy : AiAttack
{
    enum ChangeSelectAttackPatternType { Random, InOrder}


    [Header("Basic Enemy Attack Pattern")]
    [SerializeField] int numberAttackPattern = 1;
    [SerializeField] int selectAttackPattern = -1;
    [SerializeField] ChangeSelectAttackPatternType changeSelectAttackPatternType = ChangeSelectAttackPatternType.InOrder;

    [SerializeField] Sprite placeHolderAttackSprite;
    protected override void Start()
    {
        base.Start();

        //Debug.Log($"This enemy has {numberAttackPattern} attack pattern");
    }

    protected override void Update()
    {
        if (aiStat.isAttacking) return;

        float horizontalVel = rb.velocity.x;
        if (Mathf.Abs(horizontalVel) > 0.1f)
        {
            bool facingRight = horizontalVel > 0;
            spriteRenderer.flipX = !facingRight;
        }

        attackCurrentCooldown += Time.deltaTime;

        if (attackCurrentCooldown >= GetEffectiveAttackCooldown()
            && CheckTargetRange()
            && CheckForTarget())
        {
            attackCurrentCooldown = 0;

            if (numberAttackPattern > 1)
            {
                ChangeAttackPattern();
                CheckAttackDelay(selectAttackPattern);
                CheckAttackPatternType(selectAttackPattern);
            }
            else 
            {
                CheckAttackDelay(0);
                CheckAttackPatternType(0);
            }

        }
       
    }

    private void ChangeAttackPattern()
    {
        int newIndex = 0;
        switch (changeSelectAttackPatternType)
        {
            case ChangeSelectAttackPatternType.Random:
                newIndex = Random.Range(0, numberAttackPattern);
                if (newIndex == selectAttackPattern)
                {
                    // plus or minus by 1 and avoid more or less than the range
                    
                }
                break;

            case ChangeSelectAttackPatternType.InOrder:
                newIndex = selectAttackPattern++;

                break;
        }
    }

    private void CheckAttackPatternType(int index)
    {
        //Debug.Log($"Attacking by {attackPatternTypes[index]}");

        switch (attackPatternTypes[index])
        {
            case AttackPatternType.Shoot:
                CheckAttackDelay(index);
                ShootPattern(attackPattern[index], GetDirToTarget(target));
                break;

            case AttackPatternType.Lunge:
                CheckAttackDelay(index);
                LungeAttack(GetDirToTarget(target), lungeDistance, lungeSpeed, attackDelaySprite[index]);
                break;
        }

    }

    private void CheckAttackDelay(int index)
    {
        if (attackDelay[index] > 0) StartCoroutine(AttackDelay(index));
    }

    private IEnumerator AttackDelay(int index)
    {
        aiAnimation.ChangeAnimationSpeed(0f);
        aiAnimation.OverrideSprite(attackDelaySprite[index]);

        yield return new WaitForSeconds(attackDelay[index]);

    }
}
