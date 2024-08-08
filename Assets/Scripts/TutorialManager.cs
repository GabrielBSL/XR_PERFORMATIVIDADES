using System.Collections;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] bool toggle;
    private void Update()
    {
        if(toggle)
        {
            toggle = false;
            TutorialEvents.onTriggerTutorial?.Invoke();
        }
    }    
}
