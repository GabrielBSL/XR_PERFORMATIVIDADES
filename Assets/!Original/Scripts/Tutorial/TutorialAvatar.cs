using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAvatar : MonoBehaviour
{   
    [SerializeField] private float fadeInDuration;
    [SerializeField] private float fadeOutDuration;
    [SerializeField] private Material material;
    private bool isFadeCoroutineRunning = false;
    private IEnumerator FadeInCoroutine()
    {
        isFadeCoroutineRunning = true;
        for(float f = 0f; f <= 1f; f += Time.deltaTime/fadeInDuration)
        {
            material.SetFloat("_dissolve", f);
            yield return null;
        }
        isFadeCoroutineRunning = false;
    }
    private IEnumerator FadeOutCoroutine()
    {
        isFadeCoroutineRunning = true;
        for(float f = 1f; f >= 0f; f -= Time.deltaTime/fadeOutDuration)
        {
            material.SetFloat("_dissolve", f);
            yield return null;
        }
        isFadeCoroutineRunning = false;
        this.gameObject.SetActive(true);
    }

    public void FadeIn()
    {
        if(!isFadeCoroutineRunning)
        {
            StartCoroutine(FadeInCoroutine());
        }
        else Debug.Log("FadeInCoroutine already in progress!");
    }
    public void FadeOut()
    {
        if(!isFadeCoroutineRunning)
        {
            StartCoroutine(FadeOutCoroutine());
        }
        else Debug.Log("FadeOutCoroutine already in progress!");
    }
}
