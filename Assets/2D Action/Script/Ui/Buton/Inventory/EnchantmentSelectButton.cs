using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantmentSelectButton : MonoBehaviour
{
    private InventoryUiManager inventoryUiManager;
    //private EnchantmentManager enchantmentManager = GameManager.Instance.enchantmentManager;

    private int enechentmentIndex;

    // Start is called before the first frame update
    void Start()
    {
        inventoryUiManager = GameManager.Instance.inventoryUiManager;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClick()
    {
        inventoryUiManager.selectWeapon = this.name;
        Debug.Log($"{inventoryUiManager.name} select weapon is {inventoryUiManager.selectWeapon}");
    }
}
