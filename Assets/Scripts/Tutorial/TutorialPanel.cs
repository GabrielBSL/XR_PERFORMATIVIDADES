using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] private Image panel;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeInDuration;
    [SerializeField] private float fadeOutDuration;

    [Header("Fade After timeout")]
    [SerializeField] private bool fadeAfterTimeout;
    [SerializeField] private CenteredProgressBar centeredProgressBar;
    [SerializeField] private float timeoutDuration;

    private bool isVisible = false;
    private bool isFadeCoroutineRunning = false;
   
    private IEnumerator FadeInCoroutine()
    {
        isFadeCoroutineRunning = true;
        centeredProgressBar.gameObject.SetActive(false);
        centeredProgressBar.value = 1f;
        for(float f = 0f; f <= 1f; f += Time.deltaTime/fadeInDuration)
        {
            panel.material.SetFloat("_dissolve", f);
            canvasGroup.alpha = f;
            yield return null;
        }
        canvasGroup.alpha = 1f; //Para corrigir erros de arredondamento
        isVisible = true;
        isFadeCoroutineRunning = false;

        if(fadeAfterTimeout) StartCoroutine(TimeoutCoroutine());
    }
    private IEnumerator TimeoutCoroutine()
    {
        centeredProgressBar.gameObject.SetActive(true);
        for(float f = 1f; f >= 0f; f -= Time.deltaTime/timeoutDuration)
        {
            centeredProgressBar.value = f;
            yield return null;
        }
        if(isVisible && !isFadeCoroutineRunning) StartCoroutine(FadeOutCoroutine());
    }
    private IEnumerator FadeOutCoroutine()
    {
        isFadeCoroutineRunning = true;
        centeredProgressBar.gameObject.SetActive(false);
        for(float f = 1f; f >= 0f; f -= Time.deltaTime/fadeOutDuration)
        {
            panel.material.SetFloat("_dissolve", f);
            canvasGroup.alpha = f;
            yield return null;
        }
        canvasGroup.alpha = 0f; //Para corrigir erros de arredondamento
        isVisible = false;
        isFadeCoroutineRunning = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        //other.gameObject.CompareTag("") && 
        if(isVisible) StartCoroutine(FadeOutCoroutine());
    }
    public void FadeIn()
    {
        if(!isFadeCoroutineRunning)
        {
            StartCoroutine(FadeInCoroutine());
        }
        else Debug.Log("Fade Coroutine already in progress!");
    }
    //---------------------------------------------------
}
