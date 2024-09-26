using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class GameInput : MonoBehaviour
{
    private PlayerInputAction playerInputActions;
    private Vector2 currentMouseVector; // Mantieni lo stato attuale dell'input del mouse
    

    private void Awake()
    {
        playerInputActions = new PlayerInputAction();
        playerInputActions.Player.Enable();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();


        inputVector = inputVector.normalized;

        return inputVector;
    }

    public Vector2 GetMouseVectorNormalized()
    {
        Vector2 mouseVector = playerInputActions.Player.Look.ReadValue<Vector2>();

        
        currentMouseVector.x = mouseVector.x * Time.deltaTime;
        currentMouseVector.y = mouseVector.y * Time.deltaTime;

        return currentMouseVector;
    }
}
