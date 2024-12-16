using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial1 : MonoBehaviour
{
    [SerializeField] private TutorialPanel tutorialPanel;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Jangada"))
        {
            
            tutorialPanel.FadeIn();
        }
    }
}