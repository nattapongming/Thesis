using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUiManager : MonoBehaviour
{
    public string selectWeapon;
    public string selectEnchantment;

    private string enchantmentName;
    private GameObject enchantmentButton;

    private float quickSuccesstionTimer = 0.75f;
    private float quickSuccesstionDelay = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        EquipCountdown();
    }

    //If click the same enchantment in quick succession, try equip or unequip
    public void TryEquipEnchament(string name, GameObject button)
    {
        // when click new button
        if (name != enchantmentName)
        {
            enchantmentName = name;
            HighLightButton(button);
            StartEquipCountDown();
        }
        // when click same button in quick succession
        else if (quickSuccesstionDelay > 0)
        {

        }
        // or click to unselect 
        else
        {
            HighLightButton();
            enchantmentName = "";
        }
    }

    private void HighLightButton(GameObject button = null)
    {
        if (button != null && enchantmentButton)
        {
            enchantmentButton.SetActive(true);
        }
        else
        {
            enchantmentButton = button;
            enchantmentButton.SetActive(false);
        }
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
