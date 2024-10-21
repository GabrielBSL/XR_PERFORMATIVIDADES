using System;
using UnityEngine;
using FMOD.Studio;

public class FMODGestureManager : MonoBehaviour
{
    //================ VARIABLE DECLARATIONS ================

    // FMOD
    private static FMODUnity.EventReference musicEventReference;
    public static FMOD.Studio.EventInstance musicEventInstance;

    [Header("Debug")]
    [SerializeField] private GestureManager gestureManager;
    [SerializeField] [Range(0f, 1f)] private float width;
    [SerializeField] [Range(0f, 1f)] private float height;
    void Start()
    {
        // FMOD Setup
        musicEventReference = FMODUnity.RuntimeManager.PathToEventReference("event:/music");
        musicEventInstance = FMODUnity.RuntimeManager.CreateInstance(musicEventReference);
        musicEventInstance.start();
    }

    void Update()
    {
        musicEventInstance.setVolume(1f);
        musicEventInstance.setParameterByName("width", gestureManager.GetWidth());
        musicEventInstance.setParameterByName("height", gestureManager.GetHeight());
    }
}