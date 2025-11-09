using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ai;
using Unity.VisualScripting;
using Stats;

public class AiAtkSpeedModifier : MonoBehaviour
{
    [Header("Speed as time change")]
    [SerializeField] bool isAtkSpeedChangeOverTime;
    [SerializeField] float maxCooldownDiff = 1.75f;
    [SerializeField] float maxTimeForFullChange = 3f;
    float currentTimePass = 0f;

    [Header("Speed as hp change")]
    [SerializeField] bool isAtkSpeedChangeBasedOnHp;
    [SerializeField] float maxHpCooldownDiff = 1.75f;
    [SerializeField] bool fasterWhenLowHp;

    private AiStat stat;

    // Start is called before the first frame update
    void Awake()
    {
        stat = GetComponent<AiStat>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAtkSpeedChangeOverTime) currentTimePass += Time.deltaTime;
    }

    public virtual float GetCooldownReduction()
    {
        float reduction = 0f;
        if (isAtkSpeedChangeOverTime)
        {
            float timePercent = Mathf.Clamp01(currentTimePass / maxTimeForFullChange);
            reduction += timePercent * maxCooldownDiff;
        }

        if (isAtkSpeedChangeBasedOnHp)
        {
            reduction += GetHpCooldownReduction();
        }
        return reduction;
    }

    public virtual float GetHpCooldownReduction()
    {
        if (!stat) return 0f;

        //If fasterWhenLowHp is true the lower Hp percent is, the faster the attack. Opperside effect when false.
        float hpPct = stat.curHp / stat.maxHp;
        float stressPercent = fasterWhenLowHp ? (1 - hpPct) : hpPct;
        return stressPercent * maxHpCooldownDiff;
    }
}
