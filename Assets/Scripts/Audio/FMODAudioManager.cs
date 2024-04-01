using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Events;
using UnityEngine.InputSystem;

public class FMODAudioManager : MonoBehaviour
{   
    //================ VARIABLE DECLARATIONS ================

    // FMOD
    private static FMODUnity.EventReference MusicEventReference;
    private static FMOD.Studio.EventInstance MusicEventInstance;

    //================ MONOBEHAVIOUR FUNCTIONS ================

    void Start()
    {
        // FMOD Setup
        MusicEventReference = FMODUnity.RuntimeManager.PathToEventReference("event:/music");
        MusicEventInstance = FMODUnity.RuntimeManager.CreateInstance(MusicEventReference);
        MusicEventInstance.start();
    }

    //================ FMOD Parameter Setters ================

    public static void SetVolume(double volume)
    {
        MusicEventInstance.setVolume((float)volume);
    }

    public static void SetIntensity(double value)
    {
        MusicEventInstance.setParameterByName("intensity", (float)value);
    }

    public static void SetOnirico(double value)
    {
        MusicEventInstance.setParameterByName("onirico", (float)value);
    }

    public static void SetWind(double value)
    {
        MusicEventInstance.setParameterByName("wind", (float)value);
    }

    //================ FMOD Oneshots ================

    public static void Thunder(InputAction.CallbackContext callbackContext)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/thunder");
    }
}