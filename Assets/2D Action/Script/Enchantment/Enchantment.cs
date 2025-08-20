using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnchantmentCompareable { Both, Range, Melee }

[CreateAssetMenu(fileName = "Enchantment", menuName = "Enchantment/weapon")]
public class Enchantment : ScriptableObject
{
    [Header("Enchantment Info")]
    public Sprite enchantmentSprite;
    public string enchantmentDesc;

    public string enchantmentName;
    public List<string> enchantmentDescLines;

    [Header("Enchantment Stats")]
    // General Enchantment
    public EnchantmentCompareable enchantmentCompareable = EnchantmentCompareable.Both;
    public float damagePercent = 0;
    public float manaGainPercent = 0;

    // Debuff Enchantment
    public Debuff applyDebuff;
    public float debuffTime = 1;
    public float attackSize = 1f;

    // Melee Enchantment
    public float attackCoolDownPercent = 0;

    // Range Enchantment
    public float projectileSpeedPercent = 0;
    public float projectileLifeTimePercent = 0;

    // Start is called before the first frame update

    
}
