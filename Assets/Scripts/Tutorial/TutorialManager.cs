using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] TutorialPanel test;
    public void Stage1()
    {
        //sound
    }
    public void Stage2()
    {
        //bubbles
        //activates on approach        
            //reveal panel
            //reveal bubbles
        //deactivates after the 3 bubbles are popped
            //hide panel
    }

    public void Stage3()
    {
        //activates on approach
            //reveal panel
            //reveal mimic
        //deactivates after required movement
            //hide panel
            //hide mimic
    }
}
