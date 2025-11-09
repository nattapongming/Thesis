using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enchantment
{
    public class EnchantmentSlotButton : MonoBehaviour
    {
        private InventoryUiManager inventoryUiManager;
        private EnchantmentDesc enchantmentDesc;

        void Start()
        {
            inventoryUiManager = GameManager.Instance.inventoryUiManager;
            enchantmentDesc = GetComponent<EnchantmentDesc>();
        }

        public void OnClick()
        {

        }

    }
}