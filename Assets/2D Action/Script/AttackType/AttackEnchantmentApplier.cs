using Manager;
using Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnchantmentApplier
{
    private AttackStat targetWeapon;
    private BoxCollider2D boxCollider2D;
    private bool isEnchantmentIsMainWeapon;
    private ScriptableObject[] enchantment = new ScriptableObject[5];

    private GameManager gameManager = GameManager.Instance.GetComponent<GameManager>();

    public AttackEnchantmentApplier(AttackStat targetWeapon)
    {
        this.targetWeapon = targetWeapon;
        if (targetWeapon.weaponIndex == 0)
        {
            Debug.Log($"This '{targetWeapon.name} is main weapon!'");
            isEnchantmentIsMainWeapon = true;
        }
        CheckForEnchanmentSlot();
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

    // Compare to enchantment manager and apply
    public void CheckForEnchanmentSlot()
    {
        int i = 0;
        if (isEnchantmentIsMainWeapon)
        {
            foreach(EnchantmentSO enchantment in gameManager.enchantmentManager.mainEnchantment)
            {
                this.enchantment[i] = enchantment;
                //Debug.Log($"Apply enchament {i} for {targetWeapon.name}");
                i++;
            }

        }
        else
        {
            foreach (EnchantmentSO enchantment in gameManager.enchantmentManager.secEnchantment)
            {
                this.enchantment[i] = enchantment;
                //Debug.Log($"Apply enchament {i} for {targetWeapon.name}");
                i++;
            }

        }
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

        foreach (var enchantSO in enchantment)
        {
            if (enchantSO is not EnchantmentSO enchantment) continue;
            if (!IsValidType(enchantment.enchantmentCompareable)) continue;

            // Stack enchantment effects
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

        // Clamp total scale
        float minScale = 0.1f;
        float maxScale = 5.0f;
        totalScaleMultiplier = Mathf.Clamp(totalScaleMultiplier, minScale, maxScale);

        //Debug.Log($"Change {targetWeapon.transform.localScale} to {baseScale * totalScaleMultiplier}");

        // Apply stacked stats
        targetWeapon.damage = baseDamage + (baseDamage * totalDamageBonus / 100f);
        targetWeapon.managain = baseManaGain + (baseManaGain * totalManaGainBonus / 100f);
        targetWeapon.attackCoolDown = baseCooldown + (baseCooldown * totalCooldownBonus / 100f);
        targetWeapon.transform.localScale = baseScale * totalScaleMultiplier;

        // Handle difference collider
        AdjustColliderSize(targetWeapon.gameObject, totalScaleMultiplier);
    }

    private void ApplyMeleeEnchantment(EnchantmentSO enchantment)
    {
        // leave for future modifly
    }

    private void ApplyRangeEnchantment(EnchantmentSO enchantment)
    {
        if (targetWeapon.TryGetComponent<ProjectileMovement>(out var projectileMovement))
        {
            projectileMovement.speed += (projectileMovement.speed * enchantment.projectileSpeedPercent) / 100f;
            projectileMovement.lifeTime += (projectileMovement.lifeTime * enchantment.projectileLifeTimePercent) / 100f;
        }
    }

    private void AdjustColliderSize(GameObject obj, float scaleMultiplier)
    {
        if (obj.TryGetComponent<BoxCollider2D>(out var box))
        {
            box.size *= scaleMultiplier;
        }
        else if (obj.TryGetComponent<CircleCollider2D>(out var circle))
        {
            circle.radius *= scaleMultiplier;
        }
        else if (obj.TryGetComponent<CapsuleCollider2D>(out var capsule))
        {
            capsule.size *= scaleMultiplier;
        }
        else if (obj.TryGetComponent<PolygonCollider2D>(out var polygon))
        {
            Vector2[] originalPoints = polygon.points;
            Vector2[] scaledPoints = new Vector2[originalPoints.Length];
            for (int i = 0; i < originalPoints.Length; i++)
            {
                scaledPoints[i] = originalPoints[i] * scaleMultiplier;
            }
            polygon.points = scaledPoints;
        }
        else
        {
            Debug.LogWarning($"No supported collider found on {obj.name} for scaling.");
        }
    }

    private bool IsValidType(EnchantmentCompareable compareType)
    {
        return compareType == EnchantmentCompareable.Both ||
               (compareType == EnchantmentCompareable.Melee && targetWeapon.attackType == AttackStat.AttackType.melee) ||
               (compareType == EnchantmentCompareable.Range && targetWeapon.attackType == AttackStat.AttackType.range);
    }
}
