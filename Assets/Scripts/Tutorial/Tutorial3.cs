using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial3 : MonoBehaviour
{
    [SerializeField] private TutorialPanel tutorialPanel;
    [SerializeField] private TransformSampler leftTransformSampler;
    [SerializeField] private TransformSampler rightTransformSampler;
    [SerializeField] private TransformSampler headTransformSampler;
    [SerializeField] private CenteredProgressBar progressBar;
    [SerializeField] private GameObject avatar;
    [SerializeField] private Material material;
    [SerializeField] private float requiredMovement = 20000f;
    private float totalMovement = 0f;
    private bool isComplete = false;
    private void Start()
    {
        avatar.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Jangada"))
        {
            totalMovement = 0f;
            isComplete = false;          
            progressBar.gameObject.SetActive(true);
            avatar.SetActive(true);  
            avatar.GetComponent<TutorialAvatar>().FadeIn();
            tutorialPanel.FadeIn();
        }
    }
    void Update()
    {
        totalMovement += leftTransformSampler.GetSpeed();
        totalMovement += rightTransformSampler.GetSpeed();
        totalMovement += headTransformSampler.GetSpeed();
        progressBar.value = totalMovement / requiredMovement;
        material.SetFloat("_completion", totalMovement / requiredMovement);
        if(!isComplete && totalMovement >= requiredMovement)
        {
            progressBar.gameObject.SetActive(false);
            avatar.GetComponent<TutorialAvatar>().FadeOut();
            tutorialPanel.FadeOut();
            isComplete = true;
        }
    }
}