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
    //[SerializeField] private bool jangadaMoving;
    //[SerializeField] private Text stageText;

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

    void GetAldeiaBlend(float _aldeiaBlend){aldeiaBlend = _aldeiaBlend;}
    //void GetJangadaMoving(bool _jangadaMoving){jangadaMoving = _jangadaMoving;}

    private void OnEnable()
    {
        GestureReferenceEvents.aldeiaBlend += GetAldeiaBlend;
        //GestureReferenceEvents.jangadaMoving += GetJangadaMoving;
    }

    private void OnDisable()
    {
        GestureReferenceEvents.aldeiaBlend -= GetAldeiaBlend;
        //GestureReferenceEvents.jangadaMoving -= GetJangadaMoving;
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
        //stageText.text = $"jangadaMoving: {jangadaMoving}\n aldeiaBlend: {aldeiaBlend}";

        MusicEventInstance.setVolume(volume);
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
        MusicEventInstance.setParameterByName("aldeia1Angle", Mathf.Lerp(
            this.GetComponent<Aldeia1Angle>().value,
            aldeia1Angle,
            overrideInterpolation
        ));
        /*
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