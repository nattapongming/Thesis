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
            playerAttack.StartAttack(true);
        
    }

    public void AttackOther(InputAction.CallbackContext context)
    {
        if (context.performed)
            playerAttack.StartAttack(false);
    }

}
