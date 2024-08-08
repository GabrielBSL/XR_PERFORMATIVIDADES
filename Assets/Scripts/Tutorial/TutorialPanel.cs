using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class TutorialPanel : MonoBehaviour
{
    //private CanvasGroup canvasGroup;
    [SerializeField] private CenteredProgressBar centeredProgressBar;
    [SerializeField] private Image panel;
    [SerializeField] private float fadeInDuration;
    [SerializeField] private float holdDuration;
    [SerializeField] private float fadeOutDuration;

    private bool isCoroutineRunning = false;
    private void Awake()
    {
        //canvasGroup = this.GetComponent<CanvasGroup>();
        //canvasGroup.alpha = 0f;
        //canvasGroup.alpha = 1f;
    }
    private void Trigger()
    {
        if(isCoroutineRunning)
        {
            Debug.Log("Coroutine is already in progress");
        }
        else StartCoroutine(TriggerCoroutine());
    }
    
    private IEnumerator TriggerCoroutine()
    {
        isCoroutineRunning = true;
        centeredProgressBar.value = 1f;
        //canvasGroup.interactable = true;
        for(float f = 0f; f <= 1f; f += Time.deltaTime/fadeInDuration)
        {
            //canvasGroup.alpha = f;
            panel.material.SetFloat("_dissolve", f);
            yield return null;
        }

        for(float f = 1f; f >= 0f; f -= Time.deltaTime/holdDuration)
        {
            centeredProgressBar.value = f;
            yield return null;
        }

        for(float f = 1f; f >= 0f; f -= Time.deltaTime/fadeOutDuration)
        {
            //canvasGroup.alpha = f;
            panel.material.SetFloat("_dissolve", f);
            yield return null;
        }
        //canvasGroup.interactable = false;
        isCoroutineRunning = false;
    }
    //---------------------------------------------------
    private void OnEnable()
    {
        TutorialEvents.onTriggerTutorial += Trigger;
        //Debug.Log($"enabled at {System.DateTime.Now.ToString()}");
    }
    private void OnDisable()
    {
        TutorialEvents.onTriggerTutorial -= Trigger;
        //Debug.Log($"disabled at {System.DateTime.Now.ToString()}");       
    }
}
