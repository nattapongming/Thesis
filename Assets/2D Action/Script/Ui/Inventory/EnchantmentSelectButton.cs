using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enchantment
{
    public class EnchantmentSelectButton : MonoBehaviour
    {
        private InventoryUiManager inventoryUiManager;
        private EnchantmentDesc enchantmentDesc;

        // Start is called before the first frame update
        void Start()
        {
            inventoryUiManager = GameManager.Instance.inventoryUiManager;
            enchantmentDesc = GetComponent<EnchantmentDesc>();
        }

        public void OnClick()
        {
            //inventoryUiManager.selectWeapon = this.name;
            //Debug.Log($"{inventoryUiManager.name} select weapon is {inventoryUiManager.selectWeapon}");
            inventoryUiManager.TryEquipEnchament(gameObject, enchantmentDesc.enchantmentSO);
            //Debug.Log($"{inventoryUiManager.name} select weapon is {inventoryUiManager.selectWeapon}");
        }
    }
}