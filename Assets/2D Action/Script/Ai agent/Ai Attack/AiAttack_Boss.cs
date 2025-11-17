using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using Animation;

public class AiAttack_Boss : AiAttack
{
    [Header("Boss Pattern")]
    [SerializeField] protected int numberOfPattern = 1;
    [SerializeField] protected int numberOfPhase = 3;
    [SerializeField] protected float hpPercentPerPhase = 33.34f;

    [SerializeField] protected int currentAttackPattern = 0;

    [Header("Other component")]
    [SerializeField] protected AiAnimation aiAnimation;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    virtual protected int GetRandomAttackPattern()
    {
        if (numberOfPattern <= 0) return -1;
        else if (numberOfPattern == 1) return 3;
        int newPattern;
        do
        {
            newPattern = Random.Range(0, numberOfPattern);
        } while (newPattern == currentAttackPattern && numberOfPattern > 1);

        currentAttackPattern = newPattern;
        return newPattern;
    }

    virtual protected int GetCurrentPhase()
    {
        if (aiStat == null || aiStat.maxHp <= 0) return 1;
        float hpPercent = aiStat.curHp / aiStat.maxHp;
        int phase = Mathf.FloorToInt((1f - hpPercent) * numberOfPhase) + 1;
        return Mathf.Clamp(phase, 1, numberOfPhase);
    }
    
    virtual protected IEnumerator ChangeSpriteColor(float duration)
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(duration);

        spriteRenderer.color = Color.white;
    }

    
}
