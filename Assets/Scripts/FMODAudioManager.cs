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
    // FMOD
    private static FMODUnity.EventReference MusicEventReference;
    private static FMOD.Studio.EventInstance MusicEventInstance;

    // Actions (using Input System)    
    public InputActionReference rightTriggerInputActionReference;
    public InputActionReference leftGripInputActionReference;

    // Devices (using XR Tolkit)
    private UnityEngine.XR.InputDevice leftControllerDevice;
    private UnityEngine.XR.InputDevice rightControllerDevice;

    //================ DISTANCE BETWEEN CONTROLLERS ================
    // Controller Transforms (Temporary implementation using Events)
    private Transform leftControllerTransform;
    private Transform rightControllerTransform;
    private float distance;
    
    //================ RIGHT CONTROLLER ROTATION ================
    public InputActionReference rightRotationInputActionReference;
    private Quaternion rightCurrentRotation;
    private Quaternion rightLastRotation;
    private Quaternion rightDeltaRotation;

    // Velocity
    private Vector3 leftControllerVelocity;
    private Vector3 rightControllerVelocity;
    
    //================ MAIN FUNCTIONS ================

    private void OnEnable()
    {
        MainEventsManager.leftHandTargetTransformUpdate += ReceiveLeftControllerTransform;
        MainEventsManager.rightHandTargetTransformUpdate += ReceiveRightControllerTransform;
    }

    private void OnDisable()
    {
        MainEventsManager.leftHandTargetTransformUpdate -= ReceiveLeftControllerTransform;
        MainEventsManager.rightHandTargetTransformUpdate -= ReceiveRightControllerTransform;
    }

    void Start()
    {
        // FMOD Setup
        MusicEventReference = FMODUnity.RuntimeManager.PathToEventReference("event:/music");
        MusicEventInstance = FMODUnity.RuntimeManager.CreateInstance(MusicEventReference);
        MusicEventInstance.start();

        // Controller Assignment (Not used)
        leftControllerDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
	    rightControllerDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    void Update()
    {
        //================ RIGHT CONTROLLER TRIGGER ================
        //Debug.Log($"trigger  = {rightTriggerInputActionReference.action.ReadValue<float>()}");
        SetMomentum(rightTriggerInputActionReference.action.ReadValue<float>());

        //================ DISTANCE BETWEEN CONTROLLERS ================
        distance = Vector3.Distance(leftControllerTransform.position, rightControllerTransform.position);
        //Debug.Log($"distance = {distance}");
        SetOnirico(distance);

        //================ RIGHT CONTROLLER ROTATION ================
        rightLastRotation = rightCurrentRotation;
        rightCurrentRotation = rightRotationInputActionReference.action.ReadValue<Quaternion>();
        //Debug.Log($"rotation = {rightCurrentRotation}");
        rightDeltaRotation = rightCurrentRotation * Quaternion.Inverse(rightLastRotation);
        //Debug.Log($"delta    = {rightDeltaRotation}");
        if (leftGripInputActionReference.action.ReadValue<bool>())
        {
            rightCurrentRotation = rightRotationInputActionReference.action.ReadValue<Quaternion>();
            SetRiver(Math.Abs(rightDeltaRotation.x));
        }
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

    public static void SetMomentum(float value)
    {
        MusicEventInstance.setParameterByName("momentum", value);
    }

    public static void SetRiver(float value)
    {
        MusicEventInstance.setParameterByName("river", value);
    }
}

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