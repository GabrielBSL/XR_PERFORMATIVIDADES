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
    [SerializeField] [Range(0f, 1f)] private float acceleration;
    [SerializeField] [Range(0f, 1f)] private float oneiric;
    [SerializeField] [Range(0f, 2f)] private float height = 1f;
    [SerializeField] [Range(0f, 1f)] private float flute;
    [SerializeField] [Range(0f, 5f)] private float intensity;
    [SerializeField] [Range(0f, 1f)] private float marimba;
    [SerializeField] [Range(0f, 1f)] private float rainstick;
    [SerializeField] [Range(0f, 1f)] private float shaker;
    [SerializeField] [Range(0f, 1f)] private float fauna;

    //[SerializeField] [Range(1f, 60f)] private int fps;

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
        //Application.targetFrameRate = fps;

        if (!manualOverride)
        {
            switch(stage)
            {
                case 0:
                {
                    height = this.GetComponent<Crouch>().GetValue();
                    fauna = this.GetComponent<HeadTurn>().GetValue();
                    print($"fauna = {fauna}");
                    rainstick = this.GetComponent<MotionSpeed>().GetValue();
                    acceleration = this.GetComponent<MotionSpeed2>().GetValue();
                    break;
                }
                case 1:
                {
                    height = this.GetComponent<Crouch>().GetValue();
                    break;
                }
            }
            
        }

        MusicEventInstance.setVolume(volume);
        MusicEventInstance.setParameterByName("stage", stage);

        MusicEventInstance.setParameterByName("acceleration", acceleration);
        MusicEventInstance.setParameterByName("oneiric", oneiric);
        MusicEventInstance.setParameterByName("height", height);
        MusicEventInstance.setParameterByName("flute", flute);
        MusicEventInstance.setParameterByName("intensity", intensity);
        MusicEventInstance.setParameterByName("marimba", marimba);
        MusicEventInstance.setParameterByName("rainstick", rainstick);
        MusicEventInstance.setParameterByName("shaker", shaker);
        MusicEventInstance.setParameterByName("fauna", fauna);
    }

    //================ FMOD Oneshots ================

    public static void Thunder()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/thunder");
    }

    public static void Kick()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/oneshot/kick");
    }
    
    public static void Snare()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/oneshot/snare");
    }

    public static void Clap()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/oneshot/clap");
    }
    
    public static void Shaker()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/oneshot/shaker");
    }
}