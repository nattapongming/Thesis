using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnchantment : MonoBehaviour
{
    public enum EnchantmentCompareable { Both, Range, Melee}

    // General Enchantment

    public EnchantmentCompareable enchantmentCompareable = EnchantmentCompareable.Both;
    public float damageMultiplier = 0;
    public float manaGainMultiplier = 0;

    // Debuff Enchantment
    public Debuff applyDebuff;
    public float debuffTime = 1;
    public float attackSize = 1f;

    // Melee Enchantment
    public float attackCoolDownMultiplier = 0;

    // Range Enchantment
    public float projectileSpeedMultiplier = 0;
    public float projectileLifeTimeMutliplier = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyEnchantEffect()
    {

    }
}
