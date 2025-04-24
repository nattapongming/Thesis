using Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnchantmentApplier
{
    private AttackStat targetWeapon;

    public AttackEnchantmentApplier(AttackStat targetWeapon)
    {
        this.targetWeapon = targetWeapon;

        ApplyEnchantments();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ApplyEnchantments()
    {
        // Snapshot original base stats
        float baseDamage = targetWeapon.damage;
        float baseManaGain = targetWeapon.managain;
        float baseCooldown = targetWeapon.attackCoolDown;
        Vector3 baseScale = targetWeapon.transform.localScale;

        // Accumulators for additive modifiers
        float totalDamageBonus = 0f;
        float totalManaGainBonus = 0f;
        float totalCooldownBonus = 0f;
        float totalScaleMultiplier = 1f;

        foreach (var enchantSO in targetWeapon.enchantment)
        {
            if (enchantSO is not Enchantment enchantment) continue;
            if (!IsValidType(enchantment.enchantmentCompareable)) continue;

            // Accumulate additive effects
            totalDamageBonus += enchantment.damagePercent;
            totalManaGainBonus += enchantment.manaGainPercent;
            totalCooldownBonus += enchantment.attackCoolDownPercent;
            totalScaleMultiplier *= enchantment.attackSize; // pure multiplicative scale

            // Handle enchantment-specific logic
            switch (enchantment.enchantmentCompareable)
            {
                case EnchantmentCompareable.Melee:
                    ApplyMeleeEnchantment(enchantment);
                    break;
                case EnchantmentCompareable.Range:
                    ApplyRangeEnchantment(enchantment);
                    break;
            }
        }

        // Clamp total scale to avoid extreme visuals
        float minScale = 0.1f;
        float maxScale = 5.0f;
        totalScaleMultiplier = Mathf.Clamp(totalScaleMultiplier, minScale, maxScale);

        // Apply modified stats
        targetWeapon.damage = baseDamage + (baseDamage * totalDamageBonus / 100f);
        targetWeapon.managain = baseManaGain + (baseManaGain * totalManaGainBonus / 100f);
        targetWeapon.attackCoolDown = baseCooldown + (baseCooldown * totalCooldownBonus / 100f);
        targetWeapon.transform.localScale = baseScale * totalScaleMultiplier;
    }

    private void ApplyMeleeEnchantment(Enchantment enchantment)
    {
        // leave for future modifly
    }

    private void ApplyRangeEnchantment(Enchantment enchantment)
    {
        if (targetWeapon.TryGetComponent<ProjectileMovement>(out var projectileMovement))
        {
            projectileMovement.speed += (projectileMovement.speed * enchantment.projectileSpeedPercent) / 100f;
            projectileMovement.lifeTime += (projectileMovement.lifeTime * enchantment.projectileLifeTimePercent) / 100f;
        }
    }

    private bool IsValidType(EnchantmentCompareable compareType)
    {
        return compareType == EnchantmentCompareable.Both ||
               (compareType == EnchantmentCompareable.Melee && targetWeapon.attackType == AttackStat.AttackType.melee) ||
               (compareType == EnchantmentCompareable.Range && targetWeapon.attackType == AttackStat.AttackType.range);
    }
}
