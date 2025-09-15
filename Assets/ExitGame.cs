using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class ExitGame : MonoBehaviour
{


    private PlayerInput playerInput;
    private InputAction inputActionExit;
    public GameObject but;

    private bool isCursorUnlocked = false;

    void Awake() {
        but.SetActive(true);

        playerInput = GetComponent<PlayerInput>();

        inputActionExit = playerInput.actions["Esc"];

    }

    public void OnEnable()
    {
        inputActionExit.performed += OnEscapePressed;
        inputActionExit.Enable();
    }

    public void OnDisable()
    {
        inputActionExit.performed -= OnEscapePressed;
        inputActionExit.Disable();
    }
    private void OnEscapePressed(InputAction.CallbackContext context)
    {
        if (!isCursorUnlocked)
        {
          
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isCursorUnlocked = true;
        }
        else
        {
        
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isCursorUnlocked = false;
        }
    }


    public void Exit()
    {
        Application.Quit();
    }
}
