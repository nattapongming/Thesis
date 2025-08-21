using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUiManager : MonoBehaviour
{
    private GameManager gameManager = GameManager.Instance.GetComponent<GameManager>();

    public string selectWeapon;
    public string selectEnchantment;
    
    private string enchantmentName;
    private GameObject enchantmentButton;

    private float quickSuccesstionTimer = 0.75f;
    private float quickSuccesstionDelay = 0;

    // Start is called before the first frame update
    void Start()
    {
        //gameManager = GameManager.Instance.GetComponent<GameManager>();
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
            EquipOrDeequipEnchantment();
            Debug.Log($"Equip {enchantmentName}");
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
            Debug.Log("Select button");
            enchantmentButton.SetActive(true);
        }
        else
        {
            enchantmentButton = button;
            Debug.Log("Deselect Button");
            enchantmentButton.SetActive(false);
        }
    }

    private void EquipOrDeequipEnchantment()
    {

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
