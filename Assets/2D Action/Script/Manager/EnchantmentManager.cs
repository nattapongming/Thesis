using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class EnchantmentManager : MonoBehaviour
    {
        // Player Enchentment
        [Header("Main Weapon Enchantment")]
        public ScriptableObject[] mainEnchantment = new ScriptableObject[5];

        [Header("Sec Weapon Enchantment")]
        public ScriptableObject[] secEnchantment = new ScriptableObject[5];


        // Enemy Enchantment (if I can do)

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}