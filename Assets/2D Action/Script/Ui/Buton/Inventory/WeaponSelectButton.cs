using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelectButton : MonoBehaviour
{
    private InventoryUiManager inventoryUiManager;

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
