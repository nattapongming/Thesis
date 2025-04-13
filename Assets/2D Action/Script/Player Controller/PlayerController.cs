using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Movement;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerMovement playerMovement;
    PlayerAttack playerAttack;


    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(InputAction.CallbackContext context)
    {
        playerMovement.movementInput = context.ReadValue<Vector2>();
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed
            && !playerMovement.isDashing 
            && playerMovement.canDash)
        playerMovement.StartCoroutine(playerMovement.DashCoroutine());
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
            playerAttack.StartAttack();
        
    }

    public void AttackOther(InputAction.CallbackContext context)
    {
        if (context.performed)
            playerAttack.StartAttack();
    }

    public void EquipWeapon(InputAction.CallbackContext context)
    {
        // Check which button press
        string keyPressed = "";

        if (context.performed)
            keyPressed = context.control.displayName;

        int index = keyPressed switch
        {
            "1" => 0,
            "2" => 1,
            "3" => 2,
            "4" => 3,
            "5" => 4,
            _ => -1
        };

        if (index >= 0 && index < playerAttack.playerWeaponInventory.Count)
        {
            playerAttack.curWeapon = playerAttack.playerWeaponInventory[index];
            Debug.Log($"Switched to weapon {index + 1}: {playerAttack.curWeapon.name}");
        }
    }

}
