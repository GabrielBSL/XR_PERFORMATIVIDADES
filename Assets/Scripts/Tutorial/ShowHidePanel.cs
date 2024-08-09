using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHidePanel : MonoBehaviour
{
    [SerializeField] private bool showIsTrueHideIsFalse;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(showIsTrueHideIsFalse) this.GetComponentInParent<TutorialPanel>().FadeIn();
            else this.GetComponentInParent<TutorialPanel>().FadeOut();
        }
    }
}
