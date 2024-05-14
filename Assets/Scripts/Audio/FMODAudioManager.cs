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

    [SerializeField] [Range(0f, 1f)] private float overrideInterpolation = 0f;
    [SerializeField] [Range(0f, 1f)] private float volume = 0.5f;
    [SerializeField] [Range(0, 8)] private float aldeiaBlend;
    [SerializeField] private Text stageText;

    [Header("Gestures")]
    [SerializeField] [Range(0f, 1f)] private float crouch_stand = 0.5f;
    [SerializeField] [Range(-1f, 1f)] private float west_east = 1f;
    [SerializeField] [Range(0f, 1f)] private float handsHeight;
    [SerializeField] [Range(0f, 1f)] private float leftVelocity;
    [SerializeField] [Range(0f, 1f)] private float rightVelocity;

    //================ MONOBEHAVIOUR FUNCTIONS ================

    void GetAldeiaBlend(float _aldeiaBlend){aldeiaBlend = _aldeiaBlend;}

    private void OnEnable()
    {
        GestureReferenceEvents.aldeiaBlend += GetAldeiaBlend;
    }

    private void OnDisable()
    {
        GestureReferenceEvents.aldeiaBlend -= GetAldeiaBlend;
    }

    void Start()
    {
        // FMOD Setup
        MusicEventReference = FMODUnity.RuntimeManager.PathToEventReference("event:/music");
        MusicEventInstance = FMODUnity.RuntimeManager.CreateInstance(MusicEventReference);
        MusicEventInstance.start();
    }

    void Update()
    {
        MusicEventInstance.setVolume(volume);
        MusicEventInstance.setParameterByName("aldeiaBlend", aldeiaBlend);

        //Gestures
        MusicEventInstance.setParameterByName("crouch_stand", Mathf.Lerp(
            this.GetComponent<Crouch_Stand>().value,
            crouch_stand,
            overrideInterpolation
        ));
        MusicEventInstance.setParameterByName("west_east", Mathf.Lerp(
            this.GetComponent<West_East>().value,
            west_east,
            overrideInterpolation
        ));
        MusicEventInstance.setParameterByName("handsHeight", Mathf.Lerp(
            this.GetComponent<HandsHeight>().value,
            handsHeight,
            overrideInterpolation
        ));
        MusicEventInstance.setParameterByName("leftVelocity", Mathf.Lerp(
            this.GetComponent<LeftVelocity>().value,
            leftVelocity,
            overrideInterpolation
        ));
        MusicEventInstance.setParameterByName("rightVelocity", Mathf.Lerp(
            this.GetComponent<RightVelocity>().value,
            rightVelocity,
            overrideInterpolation
        ));
    }

    //================ FMOD Oneshots ================

    /*
    public static void Thunder()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/thunder");
    }

    public static void Splash()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/oneshot/splash");
    }
    
    public static void Tap()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/oneshot/tap");
    }

    public static void Clap()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/oneshot/clap");
    }
    
    public static void Shaker()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/oneshot/shaker");
    }
    */
}