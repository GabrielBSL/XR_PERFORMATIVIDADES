using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Events;
using UnityEngine.InputSystem;

public class FMODAudioManager : MonoBehaviour
{
    public static FMODUnity.EventReference MusicEventReference;
    public static FMOD.Studio.EventInstance MusicEventInstance;

    public InputActionReference triggerInputActionReference;
    public InputActionReference rotationInputActionReference;
    private Transform transformRight;
    
    private Quaternion currentRotationRight;
    private Quaternion lastRotationRight;
    private Quaternion deltaRotationRight;

    private void OnEnable()
    {
        //MainEventsManager.leftHandTransformUpdate += ReceiveLeftArmTarget;
        MainEventsManager.rightHandTransformUpdate += ReceiveRightArmTarget;
    }

    private void OnDisable()
    {
        //MainEventsManager.leftHandTransformUpdate -= ReceiveLeftArmTarget;
        MainEventsManager.rightHandTransformUpdate -= ReceiveRightArmTarget;
    }

    void Start()
    {
        // FMOD Setup
        MusicEventReference = FMODUnity.RuntimeManager.PathToEventReference("event:/music");
        MusicEventInstance = FMODUnity.RuntimeManager.CreateInstance(MusicEventReference);
        MusicEventInstance.start();
       
        currentRotationRight = rotationInputActionReference.action.ReadValue<Quaternion>();
    }

    void Update()
    {
        /*
        currentPositionRight = transformRight.position;
        positionQueueRight.Enqueue(currentPositionRight);
        while (positionQueueRight.Count > 10) oldestPositionRight = positionQueueRight.Dequeue();
        oldestPositionRight = positionQueueRight.ToArray()[0];

        currentRotationRight = transformRight.rotation.eulerAngles;
        rotationQueueRight.Enqueue(currentRotationRight);
        while (rotationQueueRight.Count > 20) avgRotationRight -= rotationQueueRight.Dequeue() / rotationQueueRight.Count;
        //avgRotationRight = Vector3.zero;
        avgRotationRight += currentRotationRight / rotationQueueRight.Count;
        Debug.Log($"avg rotation delta = {avgRotationRight.x}");
        //deltaPositionRight = currentPositionRight - oldestPositionRight;
        //Debug.Log($"delta = {deltaPositionRight.sqrMagnitude}, sample count = {positionQueueRight.Count}");
        //SetMomentum(deltaPositionRight.sqrMagnitude);

        if ()
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/thunder");
        }
    
        */

        lastRotationRight = currentRotationRight;
        currentRotationRight = rotationInputActionReference.action.ReadValue<Quaternion>();
        Debug.Log($"rotation = {rotationInputActionReference.action.ReadValue<Quaternion>()}");

        deltaRotationRight = currentRotationRight * Quaternion.Inverse(lastRotationRight);
        Debug.Log($"delta    = {deltaRotationRight}");

        Debug.Log($"trigger  = {triggerInputActionReference.action.ReadValue<float>()}");
        SetMomentum(triggerInputActionReference.action.ReadValue<float>());
    }    
    
    //================ Unity Transforms Getters ================
    
    private void ReceiveRightArmTarget(Transform _rightArmTarget)
    {
        transformRight = _rightArmTarget;
    }

    /*
    private void ReceiveLeftArmTarget(Transform _leftArmTarget)
    {
        leftHandTransform = _leftArmTarget;
    }
    */

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

    public static void SetMomentum(float value)
    {
        MusicEventInstance.setParameterByName("momentum", value);
    }
}