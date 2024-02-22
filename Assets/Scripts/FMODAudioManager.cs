using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Events;
using UnityEngine.UI;

public class FMODAudioManager : MonoBehaviour
{
    public static FMODUnity.EventReference MusicEventReference;
    public static FMOD.Studio.EventInstance MusicEventInstance;

    private Transform rightArmTargetReference;
    private Transform leftArmTargetReference;
    private float distance;
    //[SerializeField] private Text distanceText;

    private void OnEnable()
    {
        MainEventsManager.rightHandTransformUpdate += ReceiveRightArmTarget;
        MainEventsManager.leftHandTransformUpdate += ReceiveLeftArmTarget;
    }

    private void OnDisable()
    {
        MainEventsManager.rightHandTransformUpdate -= ReceiveRightArmTarget;
        MainEventsManager.leftHandTransformUpdate -= ReceiveLeftArmTarget;
    }

    private void ReceiveRightArmTarget(Transform _rightArmTarget)
    {
        rightArmTargetReference = _rightArmTarget;
    }

    private void ReceiveLeftArmTarget(Transform _leftArmTarget)
    {
        leftArmTargetReference = _leftArmTarget;
    }

    void Start()
    {
        MusicEventReference = FMODUnity.RuntimeManager.PathToEventReference("event:/music");
        MusicEventInstance = FMODUnity.RuntimeManager.CreateInstance(MusicEventReference);
        MusicEventInstance.start();
    }

    void Update()
    {
        distance = Vector3.Distance(rightArmTargetReference.position, leftArmTargetReference.position);
        SetOnirico(distance);
        //distanceText.text = "onirico: " + distance;
        Debug.Log(distance);
    }
    
    //================ Parameters ================

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
}