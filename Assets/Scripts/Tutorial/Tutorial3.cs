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
    [SerializeField] private float requiredMovement = 20000f;
    private float totalMovement = 0f;
    private void Start()
    {
        avatar.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Jangada"))
        {
            avatar.SetActive(true);
            totalMovement = 0f;            
            tutorialPanel.FadeIn();
        }
    }
    void Update()
    {
        totalMovement += leftTransformSampler.GetSpeed();
        totalMovement += rightTransformSampler.GetSpeed();
        totalMovement += headTransformSampler.GetSpeed();
        progressBar.value = totalMovement / requiredMovement;
        if(totalMovement >= requiredMovement)
        {
            progressBar.gameObject.SetActive(false);
            avatar.SetActive(false);
            tutorialPanel.FadeOut();
        }
    }
}
