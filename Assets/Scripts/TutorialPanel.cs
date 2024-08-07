using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialPanel : MonoBehaviour
{
    private bool isVisible = true; 
    private CanvasGroup canvasGroup;
    private void Awake()
    {
        canvasGroup = this.GetComponent<CanvasGroup>();
    }
    private void ToggleVisibility()
    {
        //Debug.Log($"Trigger called by event");
        StartCoroutine(isVisible? FadeOut() : FadeIn());
    }
    private IEnumerator FadeOut()
    {
        for(float f = 1f; f >= 0f; f -= 0.125f)
        {
            //Debug.Log($"fade = {f}");
            this.GetComponent<CanvasGroup>().alpha = f;
            yield return null;
        }
        isVisible = false;
        canvasGroup.interactable = false;
    }
    private IEnumerator FadeIn()
    {
        isVisible = true;
        canvasGroup.interactable = true;
        for(float f = 0f; f <= 1f; f += 0.125f)
        {
            //Debug.Log($"fade = {f}");
            this.GetComponent<CanvasGroup>().alpha = f;
            yield return null;
        }
    }
    private void OnEnable()
    {
        TutorialEvents.onTriggerTutorial += ToggleVisibility;
        //Debug.Log($"enabled at {System.DateTime.Now.ToString()}");
    }
    private void OnDisable()
    {
        TutorialEvents.onTriggerTutorial -= ToggleVisibility;
        //Debug.Log($"disabled at {System.DateTime.Now.ToString()}");       
    }
}
