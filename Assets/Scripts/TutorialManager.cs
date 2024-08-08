using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] bool toggle;
    [SerializeField] private InputActionReference rightTriggerInputAction;
    
    private void Awake()
    {
        rightTriggerInputAction.action.performed += Confirm;
    }
    private void OnEnable()
    {
        rightTriggerInputAction.action.Enable();
    }
    private void OnDisable()
    {
        rightTriggerInputAction.action.Disable();
    }
    private void Update()
    {
        if(toggle)
        {
            toggle = false;
            TutorialEvents.onTriggerTutorial?.Invoke();
        }
    }
    private void Confirm(InputAction.CallbackContext callbackContext)
    {
        TutorialEvents.onTriggerTutorial?.Invoke();
    }
}