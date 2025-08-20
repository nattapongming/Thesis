using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UiController : MonoBehaviour
{
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance.GetComponent<GameManager>();
        if (!gameManager)
        {
            Debug.Log("There isn't game manager");
        } else
            Debug.Log("There's game manager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UnPause(InputAction.CallbackContext context)
    {
        if (context.performed && gameManager)
        {
            gameManager.UpdateGamePause(GamePauseType.None);
        }
    }

    public void UnInventory(InputAction.CallbackContext context)
    {
        if (context.performed && gameManager)
        {
            gameManager.UpdateGamePause(GamePauseType.None);
        }
    }
}
