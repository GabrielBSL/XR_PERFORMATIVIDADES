using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialDebugger : MonoBehaviour
{
    [SerializeField] private InputActionReference Tutorial1DebugInputAction;    
    [SerializeField] private InputActionReference Tutorial2DebugInputAction;    
    [SerializeField] private InputActionReference Tutorial3DebugInputAction;    
    [SerializeField] bool triggerTutorial1;
    [SerializeField] bool triggerTutorial2;
    [SerializeField] bool triggerTutorial3;

    [SerializeField] TutorialPanel TutorialPanel1;
    [SerializeField] TutorialPanel TutorialPanel2;
    [SerializeField] TutorialPanel TutorialPanel3;
    private void Tutorial1Debug(InputAction.CallbackContext callbackContext) {TutorialPanel1.FadeIn();}
    private void Tutorial2Debug(InputAction.CallbackContext callbackContext) {TutorialPanel2.FadeIn();}
    private void Tutorial3Debug(InputAction.CallbackContext callbackContext) {TutorialPanel3.FadeIn();}
    private void Awake()
    {
        Tutorial1DebugInputAction.action.performed += Tutorial1Debug;
        Tutorial2DebugInputAction.action.performed += Tutorial2Debug;
        Tutorial3DebugInputAction.action.performed += Tutorial3Debug;
    }
    private void OnEnable()
    {
        Tutorial1DebugInputAction.action.Enable();
        Tutorial2DebugInputAction.action.Enable();
        Tutorial3DebugInputAction.action.Enable();
    }
    private void OnDisable()
    {
        Tutorial1DebugInputAction.action.Disable();
        Tutorial2DebugInputAction.action.Disable();
        Tutorial3DebugInputAction.action.Disable();
    }
    private void Update()
    {
        if(triggerTutorial1)
        {
            triggerTutorial1 = false;
            TutorialPanel1.FadeIn();
        }
        if(triggerTutorial2)
        {
            triggerTutorial2 = false;
            TutorialPanel2.FadeIn();
        }
        if(triggerTutorial3)
        {
            triggerTutorial3 = false;
            TutorialPanel3.FadeIn();
        }
    }
}