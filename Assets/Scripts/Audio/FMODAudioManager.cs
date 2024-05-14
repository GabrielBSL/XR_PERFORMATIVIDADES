using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] [Range(0f, 1f)] private float handsUp;
    [SerializeField] [Range(-1f, 1f)] private float lookLeftRight = 1f;
    [SerializeField] [Range(-1f, 1f)] private float lookUpDown = 1f;
    //[SerializeField] [Range(-1f, 1f)] private float leftConduct = 0f;
    //[SerializeField] [Range(-1f, 1f)] private float rightConduct = 0f;
    [SerializeField] [Range(0f, 1f)] private float leftVelocity;
    [SerializeField] [Range(0f, 1f)] private float rightVelocity;

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
        MusicEventInstance.setParameterByName("lookLeftRight", Mathf.Lerp(
            this.GetComponent<LookLeftRight>().value,
            lookLeftRight,
            overrideInterpolation
        ));
        MusicEventInstance.setParameterByName("lookUpDown", Mathf.Lerp(
            this.GetComponent<LookUpDown>().value,
            lookUpDown,
            overrideInterpolation
        ));
        MusicEventInstance.setParameterByName("handsUp", Mathf.Lerp(
            this.GetComponent<HandsUp>().value,
            handsUp,
            overrideInterpolation
        ));
        /*
        MusicEventInstance.setParameterByName("leftConduct", Mathf.Lerp(
            this.GetComponent<LeftVelocity>().value,
            leftConduct,
            overrideInterpolation
        ));
        MusicEventInstance.setParameterByName("rightConduct", Mathf.Lerp(
            this.GetComponent<RightVelocity>().value,
            rightConduct,
            overrideInterpolation
        ));
        */
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