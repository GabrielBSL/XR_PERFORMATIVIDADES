using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BubblePopper : MonoBehaviour

{
    //timer que representa quanto tempo falta pra resetar
    [SerializeField] private float streakDuration = 5f;
    private float timeOfPreviousPop;
    private float timeSincePreviousPop;
    private float countdown;

    //streak inteiro
    private int streak = 0;

    //trailLength float
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private float trailTime = 0.5f;

    [SerializeField] private XRBaseController controller;

    [SerializeField] [Range(0f, 1f)] private float popHapticIntensity;
    [SerializeField] private float popHapticDuration;
    [SerializeField] [Range(0f, 1f)] private float streakResetHapticIntensity;
    [SerializeField] private float streakResetHapticDuration;

    private void Update()
    {
        timeSincePreviousPop = Time.time - timeOfPreviousPop;
        countdown = Mathf.Clamp(streakDuration - timeSincePreviousPop, 0, streakDuration);
        if(countdown <= 0)
        {
            if(streak > 0)
            {
                //this.GetComponent<Oscillator>().PlayNote(-12);
                controller.SendHapticImpulse(streakResetHapticIntensity, streakResetHapticDuration);
            } 
            streak = 0;
        }
        trail.time = Mathf.Lerp(trail.time, streak * trailTime, 0.05f);
        GetComponent<Renderer>().material.SetColor("Color", countdown / streakDuration * Color.black);
    }

    public void Pop(bool isTutorial)
    {
        controller.SendHapticImpulse(popHapticIntensity, popHapticDuration);
        this.GetComponent<ParticleSystem>().Play();
        if(!isTutorial)
        {
            streak += 1;
            timeOfPreviousPop = Time.time;
        }
    }
}