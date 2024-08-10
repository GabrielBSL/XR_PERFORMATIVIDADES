using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial2 : MonoBehaviour
{
    [SerializeField] private TutorialPanel tutorialPanel;
    [SerializeField] private GameObject bubble;
    [SerializeField] private int bubbleCount;
    [SerializeField] private Vector3[] bubblePositions;
    [SerializeField] private List<GameObject> bubbles;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Jangada"))
        {
            bubbles = new List<GameObject>();
            for(int i = 0; i < bubbleCount; i += 1) 
            {
                bubbles.Add(Instantiate(bubble, bubblePositions[i], Quaternion.identity));
            }
            tutorialPanel.FadeIn();
            Debug.Log(bubbles);
        }
    }
}