using Enchantment;
using Manager;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryUiManager : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private EnchantmentManager enchantmentManager;
    private PlayerAttack playerAttack;

    public GameObject selectWeapon;
    public GameObject firstSelectWeaponSlot;
    public GameObject selectEnchantment;
    
    private GameObject enchantmentButton;

    private float quickSuccesstionTimer = 0.75f;
    private float quickSuccesstionDelay = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance.GetComponent<GameManager>();
        enchantmentManager = gameManager.enchantmentManager;
        playerAttack = gameManager.player.GetComponent<PlayerAttack>();

        firstSelectWeaponSlot = selectWeapon;
    }

    // Update is called once per frame
    void Update()
    {
        EquipCountdown();
    }

    //If click the same enchantment in quick succession, try equip or unequip
    public void TryEquipEnchament(GameObject button, EnchantmentSO enchantmentSO = null, bool isTryingToEquip = true)
    {
        // when click new button
        if (enchantmentButton != button)
        {
            enchantmentButton = button;
            HighLightButton(button);
            StartEquipCountDown();
        }
        // when click same button in quick succession
        else if (quickSuccesstionDelay > 0)
        {
            Debug.Log($"Equip {enchantmentButton.name} on {selectWeapon.name}");
            if (isTryingToEquip)
            {
                EquipOrUnequipEnchantment(button, enchantmentSO);
            } else
            {
                DropEnchantment(button, enchantmentSO);
            }
            enchantmentButton = null;
        }
        // or click to unselect 
        else
        {
            HighLightButton();
            enchantmentButton = null;
        }
    }

    private void HighLightButton(GameObject button = null)
    {
        if (button != null && enchantmentButton)
        {
            Debug.Log("Select button");
        }
        else
        {
            enchantmentButton = button;
            Debug.Log("Deselect Button");
        }
    }

    private void EquipOrUnequipEnchantment(GameObject button, EnchantmentSO enchantmentSO)
    {
        if (selectWeapon == null || enchantmentSO == null) return; // Safety check

        // Check for weapon type if SelectWeapon(Gameobject) have desire tag?

        string weaponType = selectWeapon.tag;
        ScriptableObject[] targetArray = weaponType == "MainWeapon"
            ? enchantmentManager.mainEnchantment
            : enchantmentManager.secEnchantment;

        if (targetArray == null)
        {
            Debug.LogError("Invalid weapon type tag on selectWeapon!");
            return;
        }

        int targetIndex = FindEquippedIndex(targetArray, enchantmentSO);

        if (targetIndex != -1)
        {
            // Unequip
            Debug.Log($"Unequip {enchantmentSO} at index {targetIndex} for {weaponType}");
            targetArray[targetIndex] = null;
        }
        else
        {
            // Equip to first empty slot
            int emptyIndex = FindEmptySlot(targetArray);
            if (emptyIndex != -1)
            {
                Debug.Log($"Equip {enchantmentSO} at index {emptyIndex} for {weaponType}");
                targetArray[emptyIndex] = enchantmentSO;
            }
            else
            {
                Debug.LogWarning("No empty slots available for " + weaponType);
            }
        }

        playerAttack.UpdateWeaponSlot();
        
    }

    private void DropEnchantment(GameObject button, EnchantmentSO enchantmentSO)
    {
        EnchantmentDesc selectSlot = button.GetComponent<EnchantmentDesc>();
        if (!selectSlot)
        {
            Debug.LogWarning($"warning! {button.name} don't have EnchantmentDesc component!");
            return;
        }

        selectSlot.UpdateDesc();
    }

    private int FindEquippedIndex(ScriptableObject[] array, EnchantmentSO enchantmentSO)
    {
        for (int i = 0; i < array.Length; i++) // Use array.Length for flexibility (e.g., change from 5)
        {
            //Debug.Log($"Compare {array[i]} with {enchantmentSO}");
            if (array[i] == enchantmentSO)
            {
                //Debug.Log($"Found the same enchantment at {i} index");
                return i;
            }
        }
        return -1;
    }

    private int FindEmptySlot(ScriptableObject[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    private void StartEquipCountDown()
    {
        quickSuccesstionDelay = quickSuccesstionTimer;
    }

    private void EquipCountdown()
    {
        if(quickSuccesstionDelay > 0)
        quickSuccesstionDelay -= Time.unscaledDeltaTime;
    }


}
