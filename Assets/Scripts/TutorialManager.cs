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

    private IEnumerator Coroutine()
    {
        for(int i = 0; i < 10; i += 1)
        {
            Debug.Log("Do something");
            yield return null;
        }
    }
}
