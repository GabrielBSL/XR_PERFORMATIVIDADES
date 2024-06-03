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

    [Header("Gestures")]
    [SerializeField] [Range(0f, 1f)] private float crouch = 0.5f;
    [SerializeField] [Range(0f, 1f)] private float handsHeight;
    [SerializeField] [Range(-1f, 1f)] private float lookUpDown = 1f;
    [SerializeField] [Range(-1f, 1f)] private float lookLeftRight = 1f;
    [SerializeField] [Range(0f, 1f)] private float leftVelocity;
    [SerializeField] [Range(0f, 1f)] private float rightVelocity;
    [SerializeField] [Range(-1f, 1f)] private float aldeia1Angle;
    [SerializeField] [Range(-1f, 1f)] private float aldeia2Angle;
    [SerializeField] [Range(-1f, 1f)] private float aldeia3Angle;
    [SerializeField] [Range(-1f, 1f)] private float leftConduct = 0f;

    //================ MONOBEHAVIOUR FUNCTIONS ================

    public static void SetFadeVolume(float _fadeVolume){fadeVolume = _fadeVolume;}

    public static void Reset()
    {
        FMODAudioManager.MusicEventInstance.stop(STOP_MODE.ALLOWFADEOUT);
    }

    private void GetAldeiaBlend(float _aldeiaBlend){aldeiaBlend = _aldeiaBlend;}

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
        MusicEventReference = FMODUnity.RuntimeManager.PathToEventReference("event:/music_standalone");
        MusicEventInstance = FMODUnity.RuntimeManager.CreateInstance(MusicEventReference);
        MusicEventInstance.start();
    }

    void Update()
    {
        MusicEventInstance.setVolume(fadeVolume);
        MusicEventInstance.setParameterByName("aldeiaBlend", aldeiaBlend);

        //Gestures
        MusicEventInstance.setParameterByName("crouch", Mathf.Lerp(
            this.GetComponent<Crouch>().value,
            crouch,
            overrideInterpolation
        ));
        MusicEventInstance.setParameterByName("handsHeight", Mathf.Lerp(
            this.GetComponent<HandsHeight>().value,
            handsHeight,
            overrideInterpolation
        ));
        MusicEventInstance.setParameterByName("lookUpDown", Mathf.Lerp(
            this.GetComponent<LookUpDown>().value,
            lookUpDown,
            overrideInterpolation
        ));
        MusicEventInstance.setParameterByName("lookLeftRight", Mathf.Lerp(
            this.GetComponent<LookLeftRight>().value,
            lookLeftRight,
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
        /*
        MusicEventInstance.setParameterByName("aldeia1Angle", Mathf.Lerp(
            this.GetComponent<Aldeia1Angle>().value,
            aldeia1Angle,
            overrideInterpolation
        ));
        MusicEventInstance.setParameterByName("aldeia2Angle", Mathf.Lerp(
            this.GetComponent<Aldeia2Angle>().value,
            aldeia2Angle,
            overrideInterpolation
        ));
        MusicEventInstance.setParameterByName("aldeia3Angle", Mathf.Lerp(
            this.GetComponent<Aldeia3Angle>().value,
            aldeia3Angle,
            overrideInterpolation
        ));
        */
        MusicEventInstance.setParameterByName("leftConduct", Mathf.Lerp(
            this.GetComponent<LeftConduct>().value,
            leftConduct,
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