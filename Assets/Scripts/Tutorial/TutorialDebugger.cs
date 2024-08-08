using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialDebugger : MonoBehaviour
{
    /*
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
    */
    [SerializeField] bool test;
    [SerializeField] bool triggerTutorial1;
    [SerializeField] bool triggerTutorial2;
    [SerializeField] bool triggerTutorial3;
    private void Update()
    {
        if(test)
        {
            test = false;
            TutorialEvents.onTest?.Invoke();
        }
        if(triggerTutorial1)
        {
            triggerTutorial1 = false;
            TutorialEvents.onTriggerTutorial1?.Invoke();
        }
        if(triggerTutorial2)
        {
            triggerTutorial2 = false;
            TutorialEvents.onTriggerTutorial2?.Invoke();
        }
        if(triggerTutorial3)
        {
            triggerTutorial3 = false;
            TutorialEvents.onTriggerTutorial3?.Invoke();
        }
    }
}