using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugNavigation : MonoBehaviour
{
    [SerializeField] private InputActionReference MoveDirectionInputAction;    
    [SerializeField] private float moveSpeed = 1f;    

    /*
    private void Awake()
    {
        MoveDirectionInputAction.action.performed += Move;
    }
    */

    private void OnEnable()
    {
        MoveDirectionInputAction.action.Enable();
    }
    private void OnDisable()
    {
        MoveDirectionInputAction.action.Disable();
    }
    private void Move(InputAction.CallbackContext callbackContext)
    {
        Vector2 direction = callbackContext.ReadValue<Vector2>();
        Vector3 velocity = moveSpeed * Time.deltaTime * new Vector3
        (
            direction.x,
            0,
            direction.y
        );
        this.transform.position += velocity;
    }

    private void Update()
    {
        Vector2 direction = MoveDirectionInputAction.action.ReadValue<Vector2>();
        Vector3 velocity = moveSpeed * Time.deltaTime * new Vector3
        (
            direction.x,
            0,
            direction.y
        );
        this.transform.position += velocity;
    }

}
