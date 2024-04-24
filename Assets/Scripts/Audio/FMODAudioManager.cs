using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FMODAudioManager : MonoBehaviour
{   
    //================ VARIABLE DECLARATIONS ================

    // FMOD
    private static FMODUnity.EventReference MusicEventReference;
    private static FMOD.Studio.EventInstance MusicEventInstance;

    [SerializeField] private bool manualOverride;
    [SerializeField] private int stage;
    [SerializeField] [Range(0f, 1f)] private float volume = 0.5f;
    [SerializeField] [Range(0f, 1f)] private float oneiric;
    [SerializeField] [Range(0f, 2f)] private float height = 1f;
    [SerializeField] [Range(0f, 1f)] private float flute;
    [SerializeField] [Range(0f, 5f)] private float intensity;
    [SerializeField] [Range(0f, 1f)] private float marimba;
    [SerializeField] [Range(0f, 1f)] private float rainstick;

    //================ MONOBEHAVIOUR FUNCTIONS ================

    void Start()
    {
        // FMOD Setup
        MusicEventReference = FMODUnity.RuntimeManager.PathToEventReference("event:/music");
        MusicEventInstance = FMODUnity.RuntimeManager.CreateInstance(MusicEventReference);
        MusicEventInstance.start();
    }

    void Update()
    {
        if (!manualOverride)
        {
            switch(stage)
            {
                case 0:
                {
                    rainstick = this.GetComponent<RainStick>().GetValue();
                    break;
                }
                case 1:
                {
                    marimba = this.GetComponent<TestGesture>().GetValue();
                    height = this.GetComponent<RiverWind>().GetValue();
                    oneiric = this.GetComponent<Fauna>().GetValue();
                    break;
                }
            }
            
        }

        MusicEventInstance.setVolume(volume);
        MusicEventInstance.setParameterByName("stage", stage);

        MusicEventInstance.setParameterByName("oneiric", oneiric);
        MusicEventInstance.setParameterByName("height", height);
        MusicEventInstance.setParameterByName("flute", flute);
        MusicEventInstance.setParameterByName("intensity", intensity);
        MusicEventInstance.setParameterByName("marimba", marimba);
        MusicEventInstance.setParameterByName("rainstick", rainstick);
    }

    //================ FMOD Oneshots ================

    public static void Thunder()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/thunder");
    }

    public static void Impulse()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/impulse");
    }
}