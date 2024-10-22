using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio;

public class FMODAudioManager : MonoBehaviour
{
    //================ VARIABLE DECLARATIONS ================

    // FMOD
    private static FMODUnity.EventReference MusicEventReference;
    public static FMOD.Studio.EventInstance MusicEventInstance;
    public static float fadeVolume = 1f;

    [Header("Debug")]
    [SerializeField] [Range(0f, 1f)] private float overrideInterpolation = 0f;
    [SerializeField] [Range(0, 8)] private float aldeiaBlend;

    //================ MONOBEHAVIOUR FUNCTIONS ================

    public static void SetFadeVolume(float _fadeVolume){fadeVolume = _fadeVolume;}

    public static void Reset()
    {
        FMODAudioManager.MusicEventInstance.stop(STOP_MODE.ALLOWFADEOUT);
    }

    public static void Play()
    {
        fadeVolume = 1f;
        FMODAudioManager.MusicEventInstance.start();
    }

    private void GetAldeiaBlend(float _aldeiaBlend){aldeiaBlend = _aldeiaBlend;}

    void Start()
    {
        // FMOD Setup
        MusicEventReference = FMODUnity.RuntimeManager.PathToEventReference("event:/music_standalone");
        MusicEventInstance = FMODUnity.RuntimeManager.CreateInstance(MusicEventReference);
        MusicEventInstance.start();
    }

    void Update()
    {
        MusicEventInstance.setVolume(fadeVolume);
        MusicEventInstance.setParameterByName("aldeiaBlend", aldeiaBlend);

        //Gestures
        /* MusicEventInstance.setParameterByName("crouch", Mathf.Lerp(
            this.GetComponent<Crouch>().value,
            crouch,
            overrideInterpolation
        )); */
    }

    //================ FMOD Oneshots ================

    public static void BubblePop()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/bubble_pop");
    }
}