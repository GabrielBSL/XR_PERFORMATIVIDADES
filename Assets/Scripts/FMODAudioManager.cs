using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class FMODAudioManager : MonoBehaviour
{   
    //================ VARIABLE DECLARATIONS ================

    // FMOD
    private static FMODUnity.EventReference MusicEventReference;
    private static FMOD.Studio.EventInstance MusicEventInstance;

    // Controller Transforms (Temporary implementation using Events)
    private Transform leftControllerTransform;
    private Transform rightControllerTransform;
    
    [SerializeField] private bool manualSettings;
    [SerializeField] [Range(0f, 1f)] private float distance;
    [SerializeField] [Range(0f, 1f)] private float height;
    

    //================ MONOBEHAVIOUR FUNCTIONS ================

    private void OnEnable()
    {
        // Get transform though event
        MainEventsManager.leftHandTransformUpdate += ReceiveLeftControllerTransform;
        MainEventsManager.rightHandTransformUpdate += ReceiveRightControllerTransform;
    }

    private void OnDisable()
    {
        MainEventsManager.leftHandTransformUpdate -= ReceiveLeftControllerTransform;
        MainEventsManager.rightHandTransformUpdate -= ReceiveRightControllerTransform;
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
        //================ DISTANCE BETWEEN CONTROLLERS ================
        if (!manualSettings)
        {
            distance = Vector3.Distance(leftControllerTransform.position, rightControllerTransform.position);
            Debug.Log($"distance = {distance}");
        }
        SetOnirico(distance);

        //================ DISTANCE BETWEEN CONTROLLERS ================
        if (!manualSettings)
        {
            height = ((leftControllerTransform.position.y + rightControllerTransform.position.y) / 2) - 1.25f;
        }
        SetWind(height);
    }    
    
    //================ Unity Transforms Getters ================

    private void ReceiveLeftControllerTransform(Transform _leftControllerTransform)
    {
        leftControllerTransform = _leftControllerTransform;
    }
    
    private void ReceiveRightControllerTransform(Transform _rightControllerTransform)
    {
        rightControllerTransform = _rightControllerTransform;
    }

    //================ FMOD Parameter Setters ================

    public static void SetVolume(float volume)
    {
        MusicEventInstance.setVolume(volume);
    }

    public static void SetIntensity(float value)
    {
        MusicEventInstance.setParameterByName("intensity", value);
    }

    public static void SetOnirico(float value)
    {
        MusicEventInstance.setParameterByName("onirico", value);
    }

    public static void SetWind(float value)
    {
        MusicEventInstance.setParameterByName("wind", value);
    }
}